using UnityEngine;

public class Leaf : MiningCells
{
    public static int LeafEnergySpend = 13;

    protected override void Awake()
    {
        base.Awake();
        CellType = 3;
        MapCreator.Tick.AddListener(leafTick);
    }
    private void leafTick()
    {
        if (CurrentMap.OrganicField.Values[CurrentPosition.x, CurrentPosition.y] > OrganicDamageTreshold || CurrentMap.ChargeField.Values[CurrentPosition.x, CurrentPosition.y] > EnergyDamageTreshold)
            OwnCharge /= 2;
        int resivedCharge = CurrentMap.IlluminationField.pickValue(CurrentPosition);
        EnergyStored += resivedCharge;
        if (resivedCharge > 2)
        { 
            EnergyAccumulated += 1;
        }
        if (Parent != null)
        {
            Parent.ReceiveEnergy(EnergyStored);
            EnergyStored = 0;
        }
        OwnCharge -= LeafEnergySpend;
        if (OwnCharge <= 0) Kill();
    }
    public override void Kill()
    {
        CurrentMap.OrganicField.AddToArea(CurrentPosition, OrganicVolume, 1);
        CurrentMap.ChargeField.AddToArea(CurrentPosition, (EnergyStored + EnergyAccumulated) / 9, 1);
        MapCreator.Tick.RemoveListener(leafTick);
        base.Kill();
    }
}
