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
        int resivedCharge = CurrentMap.IlluminationField.pickValue(CurrentPosition);
        EnergySpend += resivedCharge;
        if (resivedCharge > 2)
        { 
            EnergyAccumulated += 1;
        }
        OwnCharge -= EnergySpend;
        if (OwnCharge <= 0) Kill();
    }
    public override void Kill()
    {
        CurrentMap.OrganicField.AddToArea(CurrentPosition, OrganicVolume / 9, 1);
        CurrentMap.ChargeField.AddToArea(CurrentPosition, (EnergyStored + EnergyAccumulated) / 9, 1);
        base.Kill();
    }
}
