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
    List<IMyTerminalBlock> blockList;

    public Program()
    {
        blockList = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(blockList);

    }

    public void Main(string args)
    {
        //IMyTextPanel myTextPanel = GridTerminalSystem.GetBlockWithName("LCD00") as IMyTextPanel;
        GridTerminalSystem.SearchBlocksOfName("LCD00", blockList);
        foreach (IMyTextPanel panel in blockList)
        {
            panel.WritePublicText("222");
        }
    }

    public void Save()
    { }
}