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
    //Объявление переменных
    IMyTextPanel LcdInput;
    string LcdInputName = "[SmallSpaceMinerShip] [LCD] LCD_1";
    IMyCockpit Cockpit;
    string CockpitName = "[SmallSpaceMinerShip] Cockpit";

    public Program()
    {
        LcdInput = GridTerminalSystem.GetBlockWithName(LcdInputName) as IMyTextPanel;
        Cockpit = GridTerminalSystem.GetBlockWithName(CockpitName) as IMyCockpit;

    }

    public void Main(string args)
    {
    }

    //MyFunctions,Methods
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