using UnityEngine;

public class Creature : MonoBehaviour
{
    [HideInInspector] public int CellType = 1;
    protected Vector2Int CurrentPosition;
    protected int OwnCharge;
    protected int EnergySpend;
    protected static readonly int OrganicVolume = 90;
    protected static readonly int ChargeVolume = 40;
    protected static Map CurrentMap;
    private bool _energyWasSet = false;

    public Vector2Int Position => CurrentPosition;

    protected virtual void Awake()
    {
        CurrentPosition.x = Mathf.FloorToInt(transform.position.x);
        CurrentPosition.y = Mathf.FloorToInt(transform.position.z);
        if (!CurrentMap.AddToRegistry(this)) Kill();
    }

    public static void SetMap(Map newMap)
    {
        CurrentMap = newMap;
    }

    public virtual void Kill()
    {
        CurrentMap.RemoveFromMap(CurrentPosition);
        Destroy(gameObject);
    }

    public void setStartEnergy(int startEnergy)
    {
        if (!_energyWasSet)
        {
            OwnCharge = startEnergy;
            _energyWasSet = true;
        }
    }
}
