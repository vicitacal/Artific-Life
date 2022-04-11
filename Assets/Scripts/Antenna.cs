using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antenna : MiningCells
{
    protected override void Awake()
    {
        base.Awake();
        CellType = 2;
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
        if (Parent != null)
        {
            Parent.ReceiveEnergy(EnergyStored);
            EnergyStored = 0;
        }
        OwnCharge -= EnergySpend;
        if (OwnCharge <= 0) Kill();
    }
    public override void Kill()
    {
        CurrentMap.OrganicField.AddToArea(CurrentPosition, OrganicVolume, 1);
        CurrentMap.ChargeField.AddToArea(CurrentPosition, ((EnergyStored + EnergyAccumulated) / 9) + ChargeVolume, 1);
        MapCreator.Tick.RemoveListener(antennaTick);
        base.Kill();
    }
}
