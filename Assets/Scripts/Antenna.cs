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
        if (CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] >= 20)
        {
            CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] -= 20;
            EnergyStored += 18;
            EnergyAccumulated += 2;
        }
        else if (CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] > 0)
        {
            EnergyStored += CurentMap.Organic[CurrentPosition.x, CurrentPosition.y];
            CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] = 0;
        }
        OwnChatrge -= EnergySpend;
        if (OwnChatrge <= 0) Kill();
    }
    public override void Kill()
    {
        CurentMap.AddOrganic3x3(CurrentPosition, OrganicVolume / 9);
        CurentMap.AddEnergy3x3(CurrentPosition, (EnergyStored + EnergyAccumulated + EnergyVolume) / 9);
        base.Kill();
    }
}
