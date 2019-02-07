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
        Runtime.UpdateFrequency = UpdateFrequency.Update1;

    }

    public void Main(string args)
    {
        //IMyTextPanel myTextPanel = GridTerminalSystem.GetBlockWithName("LCD00") as IMyTextPanel;
        //{X:53596.6727987934 Y:-26798.846381723 Z:11868.6757920762};
        GridTerminalSystem.SearchBlocksOfName("LCD00", blockList);
        var pos = Me.GetPosition();
        string export = pos.ToString();
        string X = export.Remove(0, 1);
        X = X.Remove(9);
        string Y = export.Remove(0, 20);
        Y = Y.Remove(10);
        string Z = export.Remove(0, 39);
        Z = Z.Remove(9);
        foreach (IMyTextPanel panel in blockList)
        {
            panel.WritePublicText(X + "\n");
            panel.WritePublicText(Y + "\n", true);
            panel.WritePublicText(Z, true);
        }
    }

    public void Save()
    { }
}