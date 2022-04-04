using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antenna : MiningCells
{
    [HideInInspector] public const int cellType = 2;
    protected override void Awake()
    {
        base.Awake();
        EnergySpend = 12;
        MapCreator.Tick.AddListener(antennaTick);
    }
    private void antennaTick()
    {
        int resivedCharge = CurrentMap.ChargeField.TakeCharge(CurrentPosition);
        if (resivedCharge > 15)
        {
            EnergyAccumulated += resivedCharge / 10;
            EnergyStored += resivedCharge - EnergyAccumulated; 
        }
        else
        {
            EnergyStored += resivedCharge;
        }
        OwnCharge -= EnergySpend;
        if (OwnCharge <= 0) Kill();
    }
    public override void Kill()
    {
        CurrentMap.OrganicField.AddToArea(CurrentPosition, OrganicVolume / 9, 1);
        CurrentMap.ChargeField.AddToArea(CurrentPosition, (EnergyStored + EnergyAccumulated + ChargeVolume) / 9, 1);
        base.Kill();
    }
}
