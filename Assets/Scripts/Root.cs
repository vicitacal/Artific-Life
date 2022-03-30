using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MiningCells
{
    [HideInInspector] public const int cellType = 4;
    protected override void Awake()
    {
        base.Awake();
        EnergySpend = 3;
        MapCreator.Tick.AddListener(rootTick);
    }
    private void rootTick()
    {
        if (CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] >= 10)
        {
            EnergyStored += 10;
            CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] -= 10;
        }
        else if (CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] > 0)
        {
            EnergyStored += CurentMap.Organic[CurrentPosition.x, CurrentPosition.y];
            CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] = 0;
        }
        OwnChatrge -= EnergySpend;
        if (OwnChatrge <= 0) Kill();
    }
}
