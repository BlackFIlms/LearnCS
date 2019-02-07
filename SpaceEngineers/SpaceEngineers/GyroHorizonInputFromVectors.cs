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
    IMyRemoteControl RemCon;
    List<IMyGyro> gyroscopeList;

    public Program()
    {
        gyroscopeList = new List<IMyGyro>();
        GridTerminalSystem.GetBlocksOfType<IMyGyro>(gyroscopeList);
        RemCon = GridTerminalSystem.GetBlockWithName("RemCon") as IMyRemoteControl;
        Runtime.UpdateFrequency = UpdateFrequency.Update1;

    }

    public void Main(string args)
    {
        //Получаем и нормализуем вектор гравитации. Это наше направление "вниз" на планете.
        Vector3D GravityVector = RemCon.GetNaturalGravity();
        Vector3D GravNorm = Vector3D.Normalize(GravityVector);

        //Получаем проекции вектора прицеливания на все три оси блока ДУ. 
        float PitchInput = -(float)GravNorm.Dot(RemCon.WorldMatrix.Forward);
        float RollInput = (float)GravNorm.Dot(RemCon.WorldMatrix.Left);

        //На рыскание просто отправляем сигнал рыскания с контроллера. Им мы будем управлять вручную.
        float YawInput = RemCon.RotationIndicator.Y;

        //для каждого гироскопа устанавливаем текущие значения по тангажу, крену, рысканию.
        foreach (IMyGyro gyro in gyroscopeList)
        {
            gyro.GyroOverride = true;
            gyro.Yaw = YawInput;
            gyro.Roll = RollInput;
            gyro.Pitch = PitchInput;
        }

    }