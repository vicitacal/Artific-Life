using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MiningCells
{
    [HideInInspector] public const int cellType = 3;
    protected override void Awake()
    {
        base.Awake();
        EnergySpend = 15;
        MapCreator.Tick.AddListener(leafTick);
    }
    private void leafTick()
    {
        EnergyStored += CurentMap.Illumination[CurrentPosition.x, CurrentPosition.y];
        if (CurentMap.Illumination[CurrentPosition.x, CurrentPosition.y] > 2)
        { 
            EnergyAccumulated += 1;
        }
        OwnChatrge -= EnergySpend;
        if (OwnChatrge <= 0) Kill();
    }
    public override void Kill()
    {
        CurentMap.AddOrganic3x3(CurrentPosition, OrganicVolume / 9);
        CurentMap.AddEnergy3x3(CurrentPosition, (EnergyStored + EnergyAccumulated) / 9);
        base.Kill();
    }
}
