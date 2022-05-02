using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MiningCells
{
    public static int RootEnergySpend = 10;

    protected override void Awake()
    {
        base.Awake();
        CellType = 4;
        MapCreator.Tick.AddListener(rootTick);
    }
    private void rootTick()
    {
        if (CurrentMap.ChargeField.Values[CurrentPosition.x, CurrentPosition.y] > EnergyDamageTreshold)
            OwnCharge /= 2;
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
        OwnCharge -= RootEnergySpend;
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
