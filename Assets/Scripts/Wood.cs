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
        OwnCharge -= EnergySpend;
        if (OwnCharge <= 0) Kill();
    }

    public override void Kill()
    {
        CurrentMap.OrganicField.AddToArea(CurrentPosition, OrganicVolume / 9, 1);
        base.Kill();
    }
}
