using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MiningCells
{
    [HideInInspector] public const int cellType = 4;
    protected override void Awake()
    {
        base.Awake();
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
        OwnCharge -= EnergySpend;
        if (OwnCharge <= 0) Kill();
    }
    public override void Kill()
    {
        CurrentMap.OrganicField.AddToArea(CurrentPosition, (EnergyAccumulated + EnergyStored) / 9, 1);
        CurrentMap.ChargeField.AddToArea(CurrentPosition, ChargeVolume / 9, 1);
        MapCreator.Tick.RemoveListener(rootTick);
        base.Kill();
    }
}
