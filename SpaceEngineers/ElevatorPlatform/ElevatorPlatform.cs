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
    string nameRemCon = "RemCon";
    string floorSensorName = "SensFloor";
    string nameFloorPosStorage = "floorPosStorage";
    string nameLCDfloorLevel = "floorLevel"; //Надпись на этом дисплее должна быть слеедующей: "SetFloorLevel: 1". - без кавычек, цифра - номер этажа на который отправится лифт.
    string sensorName = "SensPlatform";
    string namePlatformPosStorage = "posStorage";
    string groupNameThrusters = "Engines";

    //Переменные для батарей(сенсоров).
    string sensCoord;
    string coordParsed;

    //Переменные для движков.
    List<IMyThrust> thrustList = new List<IMyThrust>();
    IMyBlockGroup thrustGroup;
    float mass;
    float gravity;
    float force;
    bool thrustOverrideState = false;
    string engineState;

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
        IMyShipController remCon = FindBlockByPartOfName(nameRemCon)[0] as IMyShipController;
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

        //Поиск блока Remote Control, получение массы и гравитации.
        if (FindBlockByPartOfName(nameRemCon).Count == 1)
        {
            IMyShipController remCon = FindBlockByPartOfName(nameRemCon)[0] as IMyShipController;
            MyShipMass rawMass = remCon.CalculateShipMass();
            mass = rawMass.PhysicalMass;

            Vector3D gravityVector = remCon.GetNaturalGravity();
            Display("GravityVector:\r\n" + gravityVector.ToString(), nameStateLCD);
            double gravityNorm = gravityVector.Normalize();
            Display("GravityNorm: " + gravityNorm.ToString(), nameStateLCD);
            gravity = Convert.ToSingle(gravityNorm);
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

        //Расчёт необходимой силы для подъёма.
        force = (gravity * mass);

        if (on_off)
        {
            Display("EnginesElevating - on", nameStateLCD);

            foreach (IMyThrust thrust in thrustList)
            {
                //Управление двигателями.
                float correction = ((force / thrustList.Count) / 100f) * (thrust.MaxThrust - thrust.MaxEffectiveThrust) / (thrust.MaxEffectiveThrust / 100f); //Поправка на высоту, для атмо двигателей. 20640
                correction = correction - ((correction / 10000f) * 15);

                thrust.ThrustOverride = (force / thrustList.Count);

                //Выводим состояние движков на панель.

                IMyShipController remCon = FindBlockByPartOfName(nameRemCon)[0] as IMyShipController;
                float g = (float)remCon.GetNaturalGravity().Length();
                float octopusGravityData = thrust.MaxEffectiveThrust / mass - g;
                
                for (int i = 0; i < CoordParser(GetTextValueMultiline(nameFloorPosStorage, floorSensorName + GetFloorNumber() + @"](?>\S+\s)+\S+")).Count; i++)
                {
                    coordParsed += CoordParser(GetTextValueMultiline(nameFloorPosStorage, floorSensorName + GetFloorNumber() + @"](?>\S+\s)+\S+"))[i].ToString();
                }

                engineState = "MET: " + thrust.MaxEffectiveThrust.ToString() + "\n";
                engineState = engineState + "MT: " + thrust.MaxThrust.ToString() + "\n";
                engineState = engineState + "CORR: " + correction.ToString() + "\n";
                engineState = engineState + "OCTOP_G_DATA: " + g.ToString() + "\n";
                engineState = engineState + "TO: " + thrust.ThrustOverride.ToString() + "\n";
                //engineState = engineState + "FM: " + ForceMultiply(CoordParser(GetTextValueMultiline(floorSensorName + GetFloorNumber(), nameFloorPosStorage))[1], CoordParser(GetTextValueMultiline(sensorName, namePlatformPosStorage))[1]).ToString();
                engineState = engineState + "FloorNum: " + GetTextValueInline("SetFloorLevel", nameLCDfloorLevel) + "\n";
                engineState = engineState + "FloorNum: " + GetFloorNumber() + "\n";
                engineState = engineState + "FloorCoords: " + GetTextValueMultiline(nameFloorPosStorage, floorSensorName + GetFloorNumber() + @"](?>\S+\s)+\S+") + "\n";
                engineState = engineState + "PlatformCoords: " + GetTextValueMultiline(namePlatformPosStorage, sensorName + @"](?>\S+\s)+\S+") + "\n";
                engineState = engineState + "CoordParser: " + CoordParser(GetTextValueMultiline(nameFloorPosStorage, floorSensorName + GetFloorNumber() + @"](?>\S+\s)+\S+")).Count + "\n";

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
                IMyRemoteControl remCon = FindBlockByPartOfName(nameRemCon)[0] as IMyRemoteControl;

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
        string lcdText = System.Text.RegularExpressions.Regex.Replace(lcd.GetPublicText(), "\r\n", "");

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
        string lcdText = lcd.GetPublicText();
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
        string pattern = @"{X:(>?-*\d*.\d*)\sY:(>?-*\d*.\d*)\sZ:(>?-*\d*.\d*)\s}";
        /*  
         *  Объявляем переменную matches.
         *  Собираем все совпадения в переменную matches.
         *  Объявляем переменную match, каждая координата будет отправлена ей в id0.
        */
        System.Text.RegularExpressions.MatchCollection matches;
        matches = System.Text.RegularExpressions.Regex.Matches(coord, pattern);
        System.Text.RegularExpressions.Match match = null;

        if (matches.Count > 0)
        {
            //id0
            match = matches[0];

            for (int i = 0; i < match.Groups.Count; i++)
            {
                float xyz;
                if (float.TryParse(match.Groups[i].ToString(), out xyz))
                {
                    coords.Add(xyz);
                }
            }
        }
        return coords;
    }
    //Получение множителя силы для движков.
    public float ForceMultiply(float Yfloor, float Yplatform)
    {
        float forceMultiply = Yfloor - Yplatform;
        if (Math.Abs(forceMultiply) <= 0.05f)
        {
            forceMultiply = 0;
        }
        return forceMultiply;
    }
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
            myTextPanel.WritePublicText(text + "\r\n", true);
        }
        else
        {
            myTextPanel.WritePublicText("");
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