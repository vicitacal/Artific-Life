using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Creature
{
    [HideInInspector] public const int cellType = 5;
    protected override void Awake()
    {
        base.Awake();
        EnergySpend = 10;
        MapCreator.Tick.AddListener(woodTick);
    }
    private void woodTick()
    {
        OwnChatrge -= EnergySpend;
        if (OwnChatrge <= 0) Kill();
    }

    public override void Kill()
    {
        CurentMap.AddOrganic3x3(CurrentPosition, OrganicVolume / 9);
        base.Kill();
    }
}
