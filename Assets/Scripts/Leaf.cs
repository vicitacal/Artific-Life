using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MiningCells
{
    public const int cellType = 3;
    protected override void Awake()
    {
        base.Awake();
        EnergySpend = 4;
        MapCreator.Tick.AddListener(leafTick);
    }
    private void leafTick()
    {
        EnergyStored += CurentMap.Illumination[CurrentPosition.x, CurrentPosition.y];
        OwnChatrge -= EnergySpend;
        if (OwnChatrge <= 0) Kill();
    }
}
