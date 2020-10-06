using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using VRageMath;
using VRage.Game;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game.EntityComponents;
using VRage.Game.Components;
using VRage.Collections;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
public sealed class Program : MyGridProgram
{
    //Объявление переменных.

    //Названия блоков.
    string nameDebugLCD = "debugLCD";
    string nameStateLCD = "stateLCD";
    string nameParserLCD = "parserLCD";
    string nameRemCon = "RemCon";
    string floorSensorName = "SensFloor";
    string nameFloorPosStorage = "floorPosStorage";
    string nameLCDfloorLevel = "floorLevel"; //Надпись на этом дисплее должна быть слеедующей: "SetFloorLevel: 1". - без кавычек, цифра - номер этажа на который отправится лифт.
    string sensorName = "SensPlatform";
    string namePlatformPosStorage = "posStorage";
    string groupNameThrusters = "Engines";

    //Блок управления.
    IMyShipController remCon;

    //Переменные для батарей(сенсоров).
    string sensCoord;
    string coordParsed;

    //Переменные для движков.
    List<IMyThrust> thrustList = new List<IMyThrust>();
    IMyBlockGroup thrustGroup;
    float mass;
    Vector3D gravityVector;
    Vector3D velocityVector;
    float gravity;
    float force;
    bool thrustOverrideState = false;
    string engineState;
    float verticalVelocity;
    float verticalProject;
    //коэфф-ты скорости и ускорения для управления тягой
    //float kV = 2;
    float kA = 4;

    //Переменные для гироскопов.
    List<IMyGyro> gyroList;
    float RollInput;
    float PitchInput;
    float YawInput;
    bool gyroStabState = false;

    public Program()
    {
        //Таймер.
        Runtime.UpdateFrequency = UpdateFrequency.Update1;
    }

    public void Main(string args)
    {
        //Обнуление состояния по умолчанию при каждом выполнении скрипта.
        Display("", nameStateLCD, true);
        Display("", nameParserLCD, true);

        //ПОЛУЧЕНИЕ КООРДИНАТ СЕНСОРОВ СТРУКТУРЫ ЧЕРЕЗ АНТЕННУ
        //Проверяем начало аргумента, если он совпадает с нашим, то выполняем скрипт для записи координат, иначе выводим это добро на debugLCD.
        if (args.StartsWith("Sensors pos"))
        {
            Display("", nameFloorPosStorage, true);
            Display(args, nameFloorPosStorage);
        }

        //КНОПКА СТАРТ/СТОП/АВАРИЯ
        else if (args.Equals("start"))
        {
            //Получение собственных координат.
            GetMyPos();
            gyroStabState = true;
            thrustOverrideState = true;
        }
        else if (args.Equals("stop"))
        {
            gyroStabState = false;
            thrustOverrideState = false;
        }
        else if (args.Equals("failure"))
        {
            gyroStabState = true;
            thrustOverrideState = false;
        }

        //НЕПОНЯТНАЯ СИТУАЦИЯ
        else
        {
            Display(args, nameDebugLCD);
        }
        
        //Управление гироскопами.
        GyroStab(gyroStabState);
        //Управление двигателями.
        ThrustOverride(groupNameThrusters, thrustOverrideState);

        //Выводим высоту над планетой на дисплей.
        remCon = FindBlockByPartOfName(nameRemCon)[0] as IMyShipController;
        double elevation;
        remCon.TryGetPlanetElevation(MyPlanetElevation.Surface, out elevation);
        //Уменьшаем кол-во знаков после запятой.
        elevation = Math.Round(elevation, 2);
        string altitudeSurf = "AltitudeSurf: " + elevation.ToString();
        Display(altitudeSurf, nameStateLCD);
        remCon.TryGetPlanetElevation(MyPlanetElevation.Sealevel, out elevation);
        //Уменьшаем кол-во знаков после запятой.
        elevation = Math.Round(elevation, 2);
        string altitudeSea = "AltitudeSea: " + elevation.ToString();
        Display(altitudeSea, nameStateLCD);

    }

    public void Save()
    { }
    
    //MyFunctions,Methods

    //ФУНКЦИИ УПРАВЛЕНИЯ ДВИЖЕНИЕМ
    //Управление тягой движков.
    public void ThrustOverride(string groupName, bool on_off)
    {
        //Берём все движки из "Engines" и закидываем их в thrustList.
        thrustGroup = GridTerminalSystem.GetBlockGroupWithName(groupName);
        thrustGroup.GetBlocksOfType<IMyThrust>(thrustList);

        gyroList = new List<IMyGyro>();
        GridTerminalSystem.GetBlocksOfType<IMyGyro>(gyroList);

        //Поиск блока Remote Control, получение массы, вектора движения и гравитации.
        if (FindBlockByPartOfName(nameRemCon).Count == 1)
        {
            remCon = FindBlockByPartOfName(nameRemCon)[0] as IMyShipController;
            MyShipMass rawMass = remCon.CalculateShipMass();
            mass = rawMass.PhysicalMass;

            gravityVector = remCon.GetNaturalGravity();
            Display("GravityVector:\r\n" + gravityVector.ToString(), nameStateLCD);
            Vector3D gravityNorm = Vector3D.Normalize(gravityVector);
            double gravityNormDouble = gravityVector.Normalize();
            Display("GravityNorm: " + gravityNormDouble.ToString(), nameStateLCD);
            gravity = Convert.ToSingle(gravityNormDouble);
            velocityVector = remCon.GetShipVelocities().LinearVelocity;
            verticalVelocity = -(float)gravityNorm.Dot(velocityVector);
        }
        else if (FindBlockByPartOfName(nameRemCon).Count > 1)
        {
            Display("Warning! \r\n We find more than \r\n 1 blocks Remote control:", nameDebugLCD);
            foreach (IMyRemoteControl remCon in FindBlockByPartOfName(nameRemCon))
            {
                string data = remCon.CustomName;
                Display(data, nameDebugLCD);
            }
        }
        else
        {
            Display("Warning! \r\n You don't have remote control blocks!", nameDebugLCD);
        }
        

        if (on_off)
        {
            Display("EnginesElevating - on", nameStateLCD);
            GetMyPos();

            /*В последний раз я переписал парсер, такое ощещение, что в предыдущий раз я вообще не понимал как он должен работать. =)
              Сейчас пытаюсь выяснить как правильно найти разницу в координатах платформы и структуры, для управления тягой движков.
              Получилось получить направление вектора вверх, но пока непонятно как это можно использовать.
              Update: Вероятно, можно получить вектор расстояния (координаты структуры - координаты платформы) и с помощью скалярного произведения
              на вектор вверх, получить нужное значение, для управления высотой.
             */
            //Достаём из панели и записываем координаты в переменные.
            string floorPos = GetTextValueMultiline(nameFloorPosStorage, floorSensorName + GetFloorNumber() + @"](?>\S+\s)+\S+");
            string PlatformPos = GetTextValueMultiline(namePlatformPosStorage, sensorName + @"](?>\S+\s)+\S+");
            //Парсим строки с координатам.(Достаём оттуда числовые значения XYZ)
            List<float> floorPosParsed = new List<float>();
            floorPosParsed = CoordParser(GetTextValueMultiline(nameFloorPosStorage, floorSensorName + GetFloorNumber() + @"](?>\S+\s)+\S+"));
            List<float> platformPosParsed = new List<float>();
            platformPosParsed = CoordParser(GetTextValueMultiline(namePlatformPosStorage, sensorName + @"](?>\S+\s)+\S+"));
            //Получаем вектор вверх и сразу парсим его.
            List<float> upVectorParsed = new List<float>();
            upVectorParsed = CoordParser(remCon.WorldMatrix.Up.ToString());Display("RemConVectorUp: " + remCon.WorldMatrix.Up.ToString(), nameParserLCD);

            //Объявляем вектор расстояния и присваиваем ему нужные значения.
            Vector3D distanceVector;
            distanceVector.X = floorPosParsed[0] - platformPosParsed[0];
            distanceVector.Y = floorPosParsed[1] - platformPosParsed[1];
            distanceVector.Z = floorPosParsed[2] - platformPosParsed[2];

            //Объявляем вектор вверх и присваиваем ему уже полученный.
            Vector3D vectorUp = remCon.WorldMatrix.Up;

            //С помощью скалярного произведения, получаем вертикальную проекцию для управления двигателями.
            verticalProject = (float)distanceVector.Dot(vectorUp);
            Display(verticalProject.ToString(),nameParserLCD);

            //Расчёт необходимой силы для подъёма. - данной силы недостаточно даже для удерживания платформы на месте
            force = (float)((1 + (verticalProject - verticalVelocity) * kA) * gravityVector.Length() * mass);

            /*"Эксперимент с батарейкой"
             * string BatVectorUP = "X:0.880934000015259 Y:-0.431211113929749 Z:0.194967776536942}";
            List<float> BatVectorUPParsed = new List<float>();
            BatVectorUPParsed = CoordParser(BatVectorUP);
            string BatPos = "X:53535.6127123895 Y:-26681.3688097174 Z:12106.9409922019}";
            //string PlPos = "X: 53534.4160012206 Y: -26683.558898142 Z: 12107.9450289915}";
            //BatPos - PlPos = X-1.2 Y-2.2 Z1
            //(BatPos - PlPos)*BatVectorUP = X - 0.96 Y 0,88 Z0,1
            List<float> BatPosParsed = new List<float>();
            BatPosParsed = CoordParser(BatPos);
            string BatPosNorm = "X: " + (BatPosParsed[0] / BatVectorUPParsed[0]).ToString() + " Y: " + (BatPosParsed[1] / BatVectorUPParsed[1]).ToString() + " Z: " + (BatPosParsed[2] / BatVectorUPParsed[2]).ToString();
            Display("BatVectorUP: " + BatVectorUP, nameParserLCD);
            Display("BatPos: " + BatPos, nameParserLCD);
            Display("BatPosNorm: " + BatPosNorm, nameParserLCD);


            foreach (IMyGyro gyro in gyroList)
            {
                Display("Gyro1VectorUp: " + gyro.WorldMatrix.Up.ToString(), nameParserLCD);
                List<float> gyroUpVectorParsed = new List<float>();
                gyroUpVectorParsed = CoordParser(gyro.WorldMatrix.Up.ToString());
                string differenceUpVectors = "X: " + (gyroUpVectorParsed[0] - upVectorParsed[0]).ToString() + " Y: " + (gyroUpVectorParsed[1] - upVectorParsed[1]).ToString() + " Z: " + (gyroUpVectorParsed[2] - upVectorParsed[2]).ToString();
                Display("DifferenceUpVectors: " + differenceUpVectors, nameParserLCD);
            }*/

            engineState = "FloorNum: " + GetTextValueInline("SetFloorLevel", nameLCDfloorLevel) + "\n";
            engineState = engineState + "FloorNum: " + GetFloorNumber() + "\n";
            engineState = engineState + "FloorCoords: " + floorPos + "\n";
            engineState = engineState + "PlatformCoords: " + PlatformPos + "\n";
            engineState = engineState + "CoordParser: " + floorPosParsed.Count + "\n";
            engineState = engineState + "CoordParsedY: " + floorPosParsed[1] + "\n";

            foreach (IMyThrust thrust in thrustList)
            {
                //Управление двигателями.
                float correction = ((force / thrustList.Count) / 100f) * (thrust.MaxThrust - thrust.MaxEffectiveThrust) / (thrust.MaxThrust / 100f); //Поправка на высоту, для атмо двигателей. 20640
                //correction = correction - ((correction / 10000f) * 15);

                thrust.ThrustOverride = (force / thrustList.Count);
                if (thrust.ThrustOverride == 0)
                    thrust.ThrustOverride = 1;

                //Выводим состояние движков на панель.

                remCon = FindBlockByPartOfName(nameRemCon)[0] as IMyShipController;
                float g = (float)remCon.GetNaturalGravity().Length();
                float octopusGravityData = thrust.MaxEffectiveThrust / mass - g;
                
                /*for (int i = 0; i < CoordParser(GetTextValueMultiline(nameFloorPosStorage, floorSensorName + GetFloorNumber() + @"](?>\S+\s)+\S+")).Count; i++)
                {
                    coordParsed += CoordParser(GetTextValueMultiline(nameFloorPosStorage, floorSensorName + GetFloorNumber() + @"](?>\S+\s)+\S+"))[i].ToString();
                }*/

                engineState = engineState + "MET: " + thrust.MaxEffectiveThrust.ToString() + "\n";
                engineState = engineState + "MT: " + thrust.MaxThrust.ToString() + "\n";
                engineState = engineState + "CORR: " + correction.ToString() + "\n";
                engineState = engineState + "OCTOP_G_DATA: " + g.ToString() + "\n";
                engineState = engineState + "TO: " + thrust.ThrustOverride.ToString() + "\n";
                //engineState = engineState + "FM: " + ForceMultiply(CoordParser(GetTextValueMultiline(floorSensorName + GetFloorNumber(), nameFloorPosStorage))[1], CoordParser(GetTextValueMultiline(sensorName, namePlatformPosStorage))[1]).ToString();
            }
            Display(engineState, nameStateLCD);
        }
        else if (!on_off)
        {
            Display("EnginesElevating - off", nameStateLCD);

            foreach (IMyThrust thrust in thrustList)
            {
                //Управление двигателями.
                thrust.ThrustOverride = 0;

                //Выводим состояние движков на панель.
                string text = thrust.ThrustOverride.ToString();
                Display(text, nameStateLCD);
            }
        }
    }

    //Стабилизатор горизонта для гироскопа.
    public void GyroStab(bool on_off)
    {

        //Находим все гироскопы и закидываем их в gyroList.
        gyroList = new List<IMyGyro>();
        GridTerminalSystem.GetBlocksOfType<IMyGyro>(gyroList);

        if (on_off)
        {
            Display("GyroStab - on", nameStateLCD);
            //Поиск блока Remote Control и получение векторов.
            if (FindBlockByPartOfName(nameRemCon).Count == 1)
            {
                //IMyRemoteControl remCon = FindBlockByPartOfName(nameRemCon) as IMyRemoteControl; - В игре вызвает исключение, поэтому использовал foreach.
                //Но потом понял, что в списках можно как и в массивах вытаскивать значение по индексу: listName[], и запихивать его в обычную переменную.
                remCon = FindBlockByPartOfName(nameRemCon)[0] as IMyRemoteControl;

                //Получаем и нормализуем вектор гравитации. Это наше направление "вниз" на планете.
                Vector3D GravityVector = remCon.GetNaturalGravity();
                Vector3D GravNorm = Vector3D.Normalize(GravityVector);

                //Получаем проекции вектора прицеливания на все три оси блока ДУ. 
                double gF = GravNorm.Dot(remCon.WorldMatrix.Forward);
                double gL = GravNorm.Dot(remCon.WorldMatrix.Left);
                double gU = GravNorm.Dot(remCon.WorldMatrix.Up);

                //Получаем сигналы по тангажу и крены операцией atan2
                RollInput = (float)Math.Atan2(gL, -gU);
                PitchInput = -(float)Math.Atan2(gF, -gU);

                //На рыскание просто отправляем сигнал рыскания с контроллера. Им мы будем управлять вручную.
                YawInput = remCon.RotationIndicator.Y;

                //для каждого гироскопа устанавливаем текущие значения по тангажу, крену, рысканию.
                foreach (IMyGyro gyro in gyroList)
                {
                    gyro.GyroOverride = true;
                    gyro.Roll = RollInput;
                    gyro.Pitch = PitchInput;
                    gyro.Yaw = YawInput;

                }
            }
            else if (FindBlockByPartOfName(nameRemCon).Count > 1)
            {
                Display("Warning! \r\n We find more than \r\n 1 blocks Remote control:", nameDebugLCD);
                foreach (IMyRemoteControl remCon in FindBlockByPartOfName(nameRemCon))
                {
                    string data = remCon.CustomName;
                    Display(data, nameDebugLCD);
                }
            }
            else
            {
                Display("Warning! \r\n You don't have remote control blocks!", nameDebugLCD);
            }
        }
        else if (!on_off)
        {
            Display("GyroStab - off", nameStateLCD);
        }
    }

    //ТЕХНИЧЕСКИЕ ФУНКЦИИ
    //Достаём текст из панели. Значение и наименование в разных строках.
    public string GetTextValueMultiline(string displayName, string pattern = @"[\S+]\S+")
    {
        string value = "";
        IMyTextPanel lcd = GridTerminalSystem.GetBlockWithName(displayName) as IMyTextPanel;
        string lcdText = System.Text.RegularExpressions.Regex.Replace(lcd.GetText(), "\r", "");
        lcdText = System.Text.RegularExpressions.Regex.Replace(lcdText, "\n", "");

        System.Text.RegularExpressions.MatchCollection matches;
        matches = System.Text.RegularExpressions.Regex.Matches(lcdText, pattern);
        System.Text.RegularExpressions.Match match = null;

        if (matches.Count > 0)
        {
            //id0
            match = matches[0];

            for (int i = 0; i < match.Groups.Count; i++)
            {
                value += match.Groups[i].ToString();
            }
        }
        return value;
    }
    //Достаём текст из панели. Значение и наименование в одной строке.
    public string GetTextValueInline(string key, string displayName)
    {
        string value = "";
        IMyTextPanel lcd = GridTerminalSystem.GetBlockWithName(displayName) as IMyTextPanel;
        string lcdText = lcd.GetText();
        string pattern = key + @":\s\d+";

        System.Text.RegularExpressions.MatchCollection matches;
        matches = System.Text.RegularExpressions.Regex.Matches(lcdText, pattern);
        System.Text.RegularExpressions.Match match = null;

        if (matches.Count > 0)
        {
            //id0
            match = matches[0];

            for (int i = 0; i < match.Groups.Count; i++)
            {
                value += match.Groups[i].ToString();
            }
        }
        return value;
    }
    //Получение целевого этажа из lcd.
    public string GetFloorNumber()
    {
        string floorNumber = "";
        floorNumber = System.Text.RegularExpressions.Regex.Replace(GetTextValueInline("SetFloorLevel", nameLCDfloorLevel), @"SetFloorLevel:\s", "");
        return floorNumber;
    }
    //Парсер координат.
    public List<float> CoordParser(string coord)
    {
        List<float> coords = new List<float>();
        coords.Clear();
        string pattern = @"X:(?>-*\d*\.\d*)\sY:(?>-*\d*\.\d*)\sZ:(?>-*\d*\.\d*)";
        /*  
         *  Объявляем переменную matches.
         *  Собираем все совпадения в переменную matches.
         *  Объявляем переменную match, первое совпадение будет отправлена ей в id0.
        */
        System.Text.RegularExpressions.MatchCollection matches;
        matches = System.Text.RegularExpressions.Regex.Matches(coord, pattern);
        System.Text.RegularExpressions.Match match = null;

        if (matches.Count > 0)
        {
            //id0
            match = matches[0];

            for (int k = 0; k < match.Groups.Count; k++)
            {
                string parsedCoords = match.Groups[k].ToString();
                string patternParsed = @"(?>-*\d*\.\d*)";

                System.Text.RegularExpressions.MatchCollection matchesParsed;
                matchesParsed = System.Text.RegularExpressions.Regex.Matches(parsedCoords, patternParsed);
                System.Text.RegularExpressions.Match matchParsed = null;

                if (matchesParsed.Count > 0)
                {
                    for (int l = 0; l < matchesParsed.Count; l++)
                    {
                        //id0
                        matchParsed = matchesParsed[l];

                        for (int n = 0; n < matchParsed.Groups.Count; n++)
                        {
                            float xyz;
                            if (float.TryParse(matchParsed.Groups[n].ToString(), out xyz))
                            {
                                coords.Add(xyz);
                            }
                        }
                    }
                }
            }
        }
        return coords;
    }
    /*//Получение множителя силы для движков. - не будет работать, т.к. нужна проекция векторов.
    public float ForceMultiply(float Yfloor, float Yplatform)
    {
        float forceMultiply = Yfloor - Yplatform;
        if (Math.Abs(forceMultiply) <= 0.05f)
        {
            forceMultiply = 0;
        }
        return forceMultiply;
    }*/
    //Поиск сенсора и запись позиции в панель.
    public void GetMyPos()
    {
        Display("", namePlatformPosStorage, true);
        if (FindBlockByPartOfName(sensorName).Count > 0)
        {
            sensCoord = ""; /*обнуление переменной*/
            //Добавляем каждую запись из цикла, в переменную без сторонних методов (coord = coord + oneString),
            //изначально юзал System.Text.StringBuilder() var.AppendLine(oneString) - тоже работает, но это сложно.
            foreach (IMyBatteryBlock sensor in FindBlockByPartOfName(sensorName))
            {
                string oneString = sensor.CustomName + "\r\n" + sensor.GetPosition().ToString() + "\r\n";

                sensCoord = sensCoord + oneString;
                Display(sensCoord, namePlatformPosStorage);
            }
        }
        else
        {
            Display("Warning! \r\n You don't have sensor blocks!", nameDebugLCD);
        }
    }
    //Показывает текст из переменной на дисплее.
    public void Display(string text, string lcdName, bool lcdClear = false)
    {
        IMyTextPanel myTextPanel = GridTerminalSystem.GetBlockWithName(lcdName) as IMyTextPanel;
        if (!lcdClear)
        {
            myTextPanel.WriteText(text + "\r\n", true);
        }
        else
        {
            myTextPanel.WriteText("");
        }
    }
    //Создаёт лист блоков в имени которых содержится текст из переменной.
    public List<IMyTerminalBlock> FindBlockByPartOfName (string blockName)
    {
        List<IMyTerminalBlock> blockList = new List<IMyTerminalBlock>();
        //GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(blockList);
        GridTerminalSystem.SearchBlocksOfName(blockName, blockList);
        return blockList;
    }

}