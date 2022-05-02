using UnityEngine;

public class Creature : MonoBehaviour
{
    [HideInInspector] public int CellType { get; protected set; } = 1;
    [HideInInspector] public Sprout Parent;
    protected Vector2Int CurrentPosition;
    protected static Map CurrentMap;
    protected int OwnCharge;
    protected int EnergySpend;
    public static int OrganicVolume = 10;
    public static int ChargeVolume = 15;
    public static int OrganicDamageTreshold = 300;
    public static int EnergyDamageTreshold = 200;
    protected bool _needToKill = false;
    private bool _energyWasSet = false;
    public Vector2Int Position => CurrentPosition;

    protected virtual void Awake()
    {
        CurrentPosition.x = Mathf.FloorToInt(transform.position.x);
        CurrentPosition.y = Mathf.FloorToInt(transform.position.z);
        if (!CurrentMap.AddToRegistry(this)) Kill();
    }
    private void Update()
    {
        if (_needToKill) Kill();
    }

    public static void SetMap(Map newMap)
    {
        CurrentMap = newMap;
    }

    public virtual void Kill()
    {
        CurrentMap.RemoveFromMap(CurrentPosition);
        CurrentMap.OrganicField.AddToArea(CurrentPosition, OrganicVolume, 1);
        CurrentMap.ChargeField.AddToArea(CurrentPosition, ChargeVolume, 1);
        Destroy(gameObject);
    }

    public virtual int Eat()
    {
        _needToKill = true;
        return OwnCharge;
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
