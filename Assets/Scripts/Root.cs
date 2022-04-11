using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MiningCells
{
    protected override void Awake()
    {
        base.Awake();
        CellType = 4;
        EnergySpend = 10;
        MapCreator.Tick.AddListener(rootTick);
    }
    private void rootTick()
    {
        int resivedCharge = CurrentMap.OrganicField.TakeOrganic(CurrentPosition);
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
        CurrentMap.OrganicField.AddToArea(CurrentPosition, ((EnergyAccumulated + EnergyStored) / 9) + OrganicVolume, 1);
        CurrentMap.ChargeField.AddToArea(CurrentPosition, ChargeVolume, 1);
        MapCreator.Tick.RemoveListener(rootTick);
        base.Kill();
    }
}
