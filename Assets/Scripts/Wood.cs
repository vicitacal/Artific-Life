using UnityEngine;

public class Wood : Creature
{
    protected override void Awake()
    {
        base.Awake();
        CellType = 5;
        EnergySpend = 10;
        MapCreator.Tick.AddListener(woodTick);
    }
    private void woodTick()
    {
        OwnCharge -= EnergySpend;
        if (OwnCharge <= 0) Kill();
    }

    public override void Kill()
    {
        CurrentMap.OrganicField.AddToArea(CurrentPosition, OrganicVolume, 1);
        base.Kill();
    }
}
