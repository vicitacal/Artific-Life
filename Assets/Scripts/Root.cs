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
        if (CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] >= 20)
        {
            CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] -= 20;
            EnergyStored += 18;
            EnergyAccumulated += 2;
        }
        else if (CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] > 0)
        {
            EnergyStored += CurentMap.Organic[CurrentPosition.x, CurrentPosition.y];
            CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] = 0;
        }
        OwnChatrge -= EnergySpend;
        if (OwnChatrge <= 0) Kill();
    }
    public override void Kill()
    {
        CurentMap.AddOrganic3x3(CurrentPosition, (EnergyAccumulated + EnergyStored) / 9);
        CurentMap.AddEnergy3x3(CurrentPosition, EnergyVolume / 9);
        base.Kill();
    }
}
