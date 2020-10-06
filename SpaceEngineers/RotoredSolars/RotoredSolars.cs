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
    IMyMotorStator rotorAz;
    IMySolarPanel solar;
    IMyTextSurface text;
    IMyTextPanel textPanel;
    const float maxPowerOut = 0.157f;
    float currentPowerOut = 0;
    float oldPowerOut = 0;
    int direction = 1;
    int start = 1;
    int counter = 0;

    public Program()
    {
        //Ищем блоки.
        rotorAz = GridTerminalSystem.GetBlockWithName("[SSB-s] RotorAzimuth") as IMyMotorStator;
        solar = GridTerminalSystem.GetBlockWithName("[SSB-s] Solar panel 1") as IMySolarPanel;
        text = Me.GetSurface(0);
        textPanel = GridTerminalSystem.GetBlockWithName("[SSB-s] LCD-Solar") as IMyTextPanel;

        Runtime.UpdateFrequency = UpdateFrequency.Update1;
    }

    public void Main(string args)
    {
        counter++;
        if (counter == 180)
        {
            //Получаем текущий выход с солярки.
            currentPowerOut = solar.CurrentOutput;
            text.WriteText("Single solar power: " + "\r\n" + currentPowerOut + " MW", false);
            textPanel.WriteText("Single solar power: " + "\r\n" + currentPowerOut + " MW", false);

            //Сравниваем текущий, максимальный и предыдущий выход с солярок.
            
            if ((currentPowerOut <= maxPowerOut) && (oldPowerOut <= currentPowerOut))
            {
                direction = 1;
                start = 1;
                text.WriteText("\r\n" + "Rotor rotating: Forward", true);
                textPanel.WriteText("\r\n" + "Rotor rotating: Forward", true);
            }
            else if ((oldPowerOut <= maxPowerOut) && (oldPowerOut > currentPowerOut))
            {
                direction *= -1;
                start = 1;
                text.WriteText("\r\n" + "Rotor rotating: Back", true);
                textPanel.WriteText("\r\n" + "Rotor rotating: Back", true);
            }
            else
            {
                direction = 1;
                start = 0;
                text.WriteText("\r\n" + "Rotor rotating: Stop", true);
                textPanel.WriteText("\r\n" + "Rotor rotating: Stop", true);
            }

            //Записывает текущее значение выхода для последующего сравнения.
            oldPowerOut = currentPowerOut;
            //Обнуляем счётчик.
            counter = 0;
        }

        //Управляем движением.
        rotorAz.TargetVelocityRPM = ((0.05f) * direction) * start;
    }

    //MyFunctions,Methods

}