using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningCells : Creature
{
    protected int EnergyStored;
    private bool _kill = false;

    private void Update()
    {
        if (_kill) Kill();
    }
    protected override void Awake()
    {
        base.Awake();
    }
    public int collectEnergy()
    {
        int boofer = EnergyStored;
        EnergyStored = 0;
        return boofer;
    }

    public int Eat()
    {
        _kill = true;
        return EnergyStored + OwnChatrge;
    }
    public override void Kill()
    {
        CurentMap.AddOrganic3x3(CurrentPosition, OrganicVolume);
        CurentMap.AddEnergy3x3(CurrentPosition, EnergyStored / 9);
        base.Kill();
    }
}
