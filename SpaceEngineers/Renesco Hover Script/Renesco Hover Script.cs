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
    //-----------------------------
    //коэфф-ты скорости и ускорения для управления тягой
    float kV = 5;
    float kA = 10;
    //коэфф-ты скорости и ускорения для управления наклоном
    float Tilt_kV = 0.1f;
    float Tilt_kA = 0.2f;
    //Объявляем нужные блоки как глоб. переменные
    IMyShipController RemCon;
    List<IMyGyro> gyroList;
    List<IMyTerminalBlock> thrList;
    List<IMyBatteryBlock> batList;

    //
    double DesiredForwardVelocity = 0;
    double ForwardAccel = 0;
    double CurrentForwardVelocity = 0;
    double OldCurrentForwardVelocity = 0;
    double CurrentElevation = 0;
    double DesiredElevation = 10;
    float DeltaElevation;
    float BatteryStored;
    float BatteryMaxStored;
    float BatteryCharged;

    //Находим блоки, устанавливаем частоту обновления
    public Program()
    {
        gyroList = new List<IMyGyro>();
        GridTerminalSystem.GetBlocksOfType<IMyGyro>(gyroList);

        thrList = new List<IMyTerminalBlock>();
        thrList = FindBlockByPartOfName("[ThrDown]");

        batList = new List<IMyBatteryBlock>();
        GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batList);

        RemCon = GridTerminalSystem.GetBlockWithName("Seat") as IMyShipController;
        
        Runtime.UpdateFrequency = UpdateFrequency.Update1;
    }

    public void Main(string argument)
    {
        //Обработка команд с контроллера.
        DesiredElevation += RemCon.MoveIndicator.Y / 5;
        DesiredForwardVelocity -= RemCon.MoveIndicator.Z / 5;

        //Обработка аргументов.
        if (argument != "")
        {
            switch (argument)
            {
                //Аргумент для полной остановки.
                case "Stop":
                    {
                        DesiredForwardVelocity = 0;
                        break;
                    }
                default:
                    break;
            }
        }

        //Получаем и нормализуем вектор гравитации. Это наше направление "вниз" на планете.
        Vector3D GravityVector = RemCon.GetNaturalGravity();
        Vector3D GravNorm = Vector3D.Normalize(GravityVector);
        Vector3D MyVelocityVector = RemCon.GetShipVelocities().LinearVelocity;

        //получаем текущую высоту и вертикальную скорость и вес
        RemCon.TryGetPlanetElevation(MyPlanetElevation.Surface, out CurrentElevation);
        DeltaElevation = (float)(DesiredElevation - CurrentElevation);

        float VerticalVelocity = -(float)MyVelocityVector.Dot(GravNorm);
        // float Weight = (float)GravityVector.Length()*RemCon.CalculateShipMass().PhysicalMass;

        //Защита крафта от падений.
        //Защита от резкого роста высоты (при пролёте над оврагами, резкий спуск с горы).
        
        //Защита от разряда баттарей.
        foreach (IMyBatteryBlock bat in batList)
        {
            BatteryStored += bat.CurrentStoredPower;
            BatteryMaxStored += bat.MaxStoredPower;
        }
        BatteryCharged = BatteryStored / (BatteryMaxStored / 100);

        if (BatteryCharged <= 8)
        {
            DesiredElevation = 0;
        }
        //Вывод сервисной информации.
        Display("", "infoLCD", true);
        Display("Speed: " + DesiredForwardVelocity + "\r\n", "infoLCD");
        Display("Height: " + DesiredElevation, "infoLCD");
        Display("DeltaElevation: " + DeltaElevation, "infoLCD");
        Display("BatteryCharged: " + BatteryCharged, "infoLCD");

        //Считаем косинус угла наклона крафта
        float TiltCos = (float)RemCon.WorldMatrix.Down.Dot(GravNorm);
        //Считаем тягу
        float Thrust = (float)((1 + (DeltaElevation * kV - VerticalVelocity) * kA) * GravityVector.Length() * RemCon.CalculateShipMass().PhysicalMass / TiltCos);
        if (Thrust <= 0)
            Thrust = 1;
        /*
        float HoverCorrection = (DeltaElevation * kV - VerticalVelocity) * kA;
        float Thrust = (float)(GravityVector.Length() * RemCon.CalculateShipMass().PhysicalMass * (1 + HoverCorrection) / TiltCos);
        if (Thrust <= 0)
            Thrust = 1;

        Echo(""+ HoverCorrection + "\n" + TiltCos + "\n"+ thrList.Count+ "\n" + DeltaElevation + "\n" + VerticalVelocity); */

        Vector3D MyVelocityVectorNorm = Vector3D.Reject(MyVelocityVector, RemCon.WorldMatrix.Forward) / 10;
        if (MyVelocityVectorNorm.Length() > 1) MyVelocityVectorNorm = Vector3D.Normalize(MyVelocityVectorNorm);

        Vector3D ForwardInput = Vector3D.Normalize(Vector3D.Reject(RemCon.WorldMatrix.Forward, GravNorm)) * RemCon.MoveIndicator.Z;
        Vector3D LeftInput = Vector3D.Normalize(Vector3D.Reject(RemCon.WorldMatrix.Left, GravNorm)) * RemCon.RollIndicator;

        //Управляем скоростью
        CurrentForwardVelocity = MyVelocityVector.Dot(Vector3D.Normalize(Vector3D.Reject(RemCon.WorldMatrix.Forward, GravNorm)));
        ForwardAccel = CurrentForwardVelocity - OldCurrentForwardVelocity;
        OldCurrentForwardVelocity = CurrentForwardVelocity;
        double VelocityDelta = DesiredForwardVelocity - CurrentForwardVelocity;

        double VelocityFactor = (VelocityDelta * Tilt_kV - ForwardAccel) * Tilt_kA;
        Vector3D TiltCorrector = -Vector3D.Normalize(Vector3D.Reject(RemCon.WorldMatrix.Forward, GravNorm)) * VelocityFactor;


        Vector3D AlignVector = Vector3D.Normalize(GravNorm + TiltCorrector + MyVelocityVectorNorm / 2 + (LeftInput) / 1.2);

        //Получаем проекции вектора прицеливания на все три оси блока ДУ. 
        float PitchInput = -(float)AlignVector.Dot(RemCon.WorldMatrix.Forward);
        float RollInput = (float)AlignVector.Dot(RemCon.WorldMatrix.Left);

        //На рыскание просто отправляем сигнал рыскания с контроллера. Им мы будем управлять вручную.
        float YawInput = RemCon.MoveIndicator.X;


        //для каждого гироскопа устанавливаем текущие значения по тангажу, крену, рысканию.
        foreach (IMyGyro gyro in gyroList)
        {
            gyro.GyroOverride = true;
            gyro.Yaw = YawInput;
            gyro.Roll = RollInput;
            gyro.Pitch = PitchInput;
        }

        //раздаем тягу на движки

        foreach (IMyThrust thr in thrList)
        {
            thr.ThrustOverride = Thrust / thrList.Count;
        }

    }
    //Показывает текст из переменной на дисплее или очищает его.
    public void Display(string text, string lcdName, bool lcdClear = false)
    {
        IMyTextPanel myTextPanel = GridTerminalSystem.GetBlockWithName(lcdName) as IMyTextPanel;
        if (!lcdClear)
        {
            StringBuilder sb = new StringBuilder();
            myTextPanel.ReadText(sb);
            sb.AppendLine(text);
            myTextPanel.WriteText(sb + "\r\n");
        }
        else
        {
            myTextPanel.WriteText("");
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
