using UnityEngine;

public class Leaf : MiningCells
{
    
    protected override void Awake()
    {
        base.Awake();
        CellType = 3;
        EnergySpend = 13;
        MapCreator.Tick.AddListener(leafTick);
    }
    private void leafTick()
    {
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
        OwnCharge -= EnergySpend;
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
