using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Creature
{
    [HideInInspector] public const int cellType = 5;
    protected override void Awake()
    {
        base.Awake();
        EnergySpend = 4;
        MapCreator.Tick.AddListener(woodTick);
    }
    private void woodTick()
    {
        OwnChatrge -= EnergySpend;
        if (OwnChatrge <= 0) Kill();
    }

}
