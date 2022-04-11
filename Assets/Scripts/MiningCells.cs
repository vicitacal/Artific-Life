
public class MiningCells : Creature
{
    protected int EnergyStored = 0;
    protected int EnergyAccumulated = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public override int Eat()
    {
        _needToKill = true;
        return EnergyStored + OwnCharge + EnergyAccumulated;
    }

    public override void Kill()
    {
        Parent.ChildsCount--;
        base.Kill();
    }
}
