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
    string debugLCD = "debugLCD";
    string nameRemCon = "RemCon";
    string namePosStorage = "posStorage";

    //Переменные для стабилзатора на гироскопах.
    List<IMyGyro> gyroList;
    float RollInput;
    float PitchInput;
    float YawInput;

    public Program()
    {
        //Таймер.
        Runtime.UpdateFrequency = UpdateFrequency.Update1;
        //Находим все гироскопы и закидываем их в gyroList.
        gyroList = new List<IMyGyro>();
        GridTerminalSystem.GetBlocksOfType<IMyGyro>(gyroList);
    }

    public void Main(string args)
    {
        //Проверяем начало аргумента, если он совпадает с нашим, то выаолняем скрипт, иначе выводим это добро на debugLCD
        if (args.StartsWith("Sensors pos"))
        {
            Display(args, namePosStorage);
        }
        else
        {
            Display(args, debugLCD);
        }

        //Поиск блока Remote Control и получение векторов.
        if (FindBlockByPartOfName(nameRemCon).Count == 1)
        {
            //IMyRemoteControl remCon = FindBlockByPartOfName(nameRemCon) as IMyRemoteControl; - В игре вызвает исключение, поэтому использую foreach.
            foreach (IMyRemoteControl remCon in FindBlockByPartOfName(nameRemCon))
            {
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
            }
        }
        else if (FindBlockByPartOfName(nameRemCon).Count > 1)
        {
            Display("Warning! \r\n We find more than \r\n 1 blocks Remote control:", debugLCD);
            foreach (IMyRemoteControl remCon in FindBlockByPartOfName(nameRemCon))
            {
                string data = remCon.CustomName;
                Display(data, debugLCD);
            }
        }
        else
        {
            Display("Warning! \r\n You don't have remote control blocks!", debugLCD);
        }

        //для каждого гироскопа устанавливаем текущие значения по тангажу, крену, рысканию.
        foreach (IMyGyro gyro in gyroList)
        {
            gyro.GyroOverride = true;
            gyro.Yaw = YawInput;
            gyro.Roll = RollInput;
            gyro.Pitch = PitchInput;
        }
    }

    public void Save()
    { }

    //MyFunctions

    //Показывает текст из переменной на дисплее.
    public void Display (string text, string lcdName)
    {
        IMyTextPanel myTextPanel = GridTerminalSystem.GetBlockWithName(lcdName) as IMyTextPanel;
        myTextPanel.WritePublicText(text + "\r\n", true);
    }
    //Создаёт лист блоков в имени которых содержится текст из переменной.
    public List<IMyTerminalBlock> FindBlockByPartOfName (string blockName)
    {
        List<IMyTerminalBlock> blockList;
        blockList = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(blockList);
        GridTerminalSystem.SearchBlocksOfName(blockName, blockList);
        return blockList;
    }

}