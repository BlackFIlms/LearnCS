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
    string antennaName = "RadioStruct";
    string sensorName = "SensFloor";
    string nameFloorPosStorage = "floorPosStorage";

    //Переменные для антенны и батарей(сенсоров).
    string sensCoord;
    string message;

    public Program()
    {

    }

    public void Main(string args)
    {
        //Поиск сенсоров, определение и запись каждой позиции в переменную
        Display("", nameFloorPosStorage, true);
        if (FindBlockByPartOfName(sensorName).Count > 0)
        {
            sensCoord = ""; /*обнуление переменной*/
            //Добавляем каждую запись из цикла, в переменную без сторонних методов (coord = coord + oneString),
            //изначально юзал System.Text.StringBuilder() var.AppendLine(oneString) - тоже работает, но это сложно.
            foreach (IMyBatteryBlock sensor in FindBlockByPartOfName(sensorName))
            {
                string oneString = sensor.CustomName + "\r\n" + sensor.GetPosition().ToString() + "\r\n";
                sensCoord = sensCoord + oneString;
            }
        }
        else
        {
            Display("Warning! \r\n You don't have sensor blocks!", nameDebugLCD);
        }

        //Поиск антенны, создание и отправка сообщения
        if (FindBlockByPartOfName(antennaName).Count == 1)
        {
            foreach (IMyRadioAntenna antenna in FindBlockByPartOfName(antennaName))
            {
                message = "Sensors pos:" + "\r\n" + sensCoord;
                Display(message, nameFloorPosStorage);
                antenna.TransmitMessage(message, MyTransmitTarget.Owned);
            }
        }
        else if (FindBlockByPartOfName(antennaName).Count > 1)
        {
            Display("Warning! \r\n We find more than \r\n 1 Antenna blocks:", nameDebugLCD);
            foreach (IMyRemoteControl remCon in FindBlockByPartOfName(antennaName))
            {
                string data = remCon.CustomName;
                Display(data, nameDebugLCD);
            }
        }
        else
        {
            Display("Warning! \r\n You don't have Antenna blocks!", nameDebugLCD);
        }
    }

    public void Save()
    { }

    //MyFunctions,Methods

    //Показывает текст из переменной на дисплее или очищает его.
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
    public List<IMyTerminalBlock> FindBlockByPartOfName(string blockName)
    {
        List<IMyTerminalBlock> blockList;
        blockList = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(blockList);
        GridTerminalSystem.SearchBlocksOfName(blockName, blockList);
        return blockList;
    }

}