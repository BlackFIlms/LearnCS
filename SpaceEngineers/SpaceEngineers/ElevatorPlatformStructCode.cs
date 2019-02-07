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
    string antennaName = "RaioStruct";
    string sensorName = "SensFloor";

    public Program()
    {

    }

    public void Main(string args)
    {
        //Поиск сенсоров, определение и запись позиции
        if (FindBlockByPartOfName(sensorName).Count > 0)
        {
            foreach (IMyBatteryBlock sensor in FindBlockByPartOfName(sensorName))
            {
                string message = "Sensors pos:" + "\r\n" + sensor.CustomName + "\r\n" + sensor.GetPosition().ToString();
                //Поиск антенны и отправка сообщения
                if (FindBlockByPartOfName(antennaName).Count == 1)
                {
                    foreach (IMyRadioAntenna antenna in FindBlockByPartOfName(antennaName))
                    {
                        antenna.TransmitMessage(message, MyTransmitTarget.Owned);
                    }
                }
                else if (FindBlockByPartOfName(antennaName).Count > 1)
                {
                    Display("Warning! \r\n We find more than \r\n 1 Antenna blocks:", debugLCD);
                    foreach (IMyRemoteControl remCon in FindBlockByPartOfName(antennaName))
                    {
                        string data = remCon.CustomName;
                        Display(data, debugLCD);
                    }
                }
                else
                {
                    Display("Warning! \r\n You don't have Antenna blocks!", debugLCD);
                }
            }
        }
        else
        {
            Display("Warning! \r\n You don't have sensor blocks!", debugLCD);
        }

    }

    public void Save()
    { }

    //MyFunctions

    //Показывает текст из переменной на дисплее.
    public void Display(string text, string lcdName)
    {
        IMyTextPanel myTextPanel = GridTerminalSystem.GetBlockWithName(lcdName) as IMyTextPanel;
        myTextPanel.WritePublicText(text + "\r\n", true);
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