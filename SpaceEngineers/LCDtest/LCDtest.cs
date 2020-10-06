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
    IMyTextPanel extPanel;
    IMyTextPanel extPanel2;
    IMyTimerBlock Timer;
    int tickCount;

    public Program()
    {
        extPanel = GridTerminalSystem.GetBlockWithName("lcdinfo") as IMyTextPanel;
        extPanel2 = GridTerminalSystem.GetBlockWithName("lcdinfo2") as IMyTextPanel;
        Timer = GridTerminalSystem.GetBlockWithName("Timer") as IMyTimerBlock;
        Timer.TriggerDelay = 1;
        Runtime.UpdateFrequency = UpdateFrequency.Update1;
    }

    public void Main(string args)
    {
        extPanel.WriteText("");
        extPanel2.WriteText("111");
        Timer.StartCountdown();
        if (args == "Tick")
        {
            tickCount++;
            extPanel.WriteText("Tack " + tickCount);
        }
    }
}