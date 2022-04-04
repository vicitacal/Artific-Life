
public class MiningCells : Creature
{
    protected int EnergyStored = 0;
    protected int EnergyAccumulated = 0;
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
        return EnergyStored + OwnCharge;
    }
}
