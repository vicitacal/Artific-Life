using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antenna : MiningCells
{
    [HideInInspector] public const int cellType = 2;
    protected override void Awake()
    {
        base.Awake();
        EnergySpend = 2;
        MapCreator.Tick.AddListener(antennaTick);
    }
    private void antennaTick()
    {
        if (CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] >= 10)
        {
            EnergyStored += 10;
            CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] -= 10;
        }
        else if (CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] > 0)
        {
            EnergyStored += CurentMap.Organic[CurrentPosition.x, CurrentPosition.y];
            CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] = 0;
        }
        OwnChatrge -= EnergySpend;
        if (OwnChatrge <= 0) Kill();
    }
}
