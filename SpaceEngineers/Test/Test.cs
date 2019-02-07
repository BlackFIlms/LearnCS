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
    //Объявление блоков и переменных
    List<IMyTerminalBlock> blockList;
    List<IMyTerminalBlock> gyroscopeList;

    // НАЧАЛО СКРИПТА
    public Program()
    {
        blockList = new List<IMyTerminalBlock>();
        gyroscopeList = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(blockList);
        GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(gyroscopeList);
        Runtime.UpdateFrequency = UpdateFrequency.Update1;

    }

    public void Main(string args)
    {
        var indexGyroNames = gyroscopeList.Capacity;
        GridTerminalSystem.SearchBlocksOfName("Konb", gyroscopeList);

        //IMyTextPanel myTextPanel = GridTerminalSystem.GetBlockWithName("LCD00") as IMyTextPanel;
        GridTerminalSystem.SearchBlocksOfName("LCD01", blockList);
        foreach (IMyTextPanel panel in blockList)
        {
            foreach (IMyGyro konb in gyroscopeList)
            {
                int i;
                for (i = 0; i < indexGyroNames; i++)
                {
                    string Orientation = konb.Orientation.ToString();
                    panel.WritePublicText(konb.DisplayNameText + "\n");
                    panel.WritePublicText(Orientation + "\n", true);
                    string iSTR = i.ToString();
                    panel.WritePublicText("i = " + iSTR + "\n", true);
                    panel.WritePublicText("konb" + "\n", true);
                }
            }
            string indexGyroNamesSTR = indexGyroNames.ToString();
            panel.WritePublicText("gyroscopeList count is " + indexGyroNamesSTR + " items", true);

            List<IMyShipController> remCon = new List<IMyShipController>();
            GridTerminalSystem.GetBlocksOfType<IMyShipController>(remCon);
            MyShipMass rawMass = remCon[0].CalculateShipMass();
            float mass = rawMass.TotalMass;

            Vector3D gravityVector = remCon[0].GetNaturalGravity();
            panel.WritePublicText("GravityVector:\r\n" + gravityVector.ToString() + "\r\n", true);
            double gravityNorm = gravityVector.Normalize();
            panel.WritePublicText("GravityNorm: " + gravityNorm.ToString() + "\r\n", true);
        }
    }

}