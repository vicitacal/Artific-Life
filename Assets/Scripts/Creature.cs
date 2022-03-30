using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [HideInInspector] public int CellType = 1;
    protected Vector2Int CurrentPosition;
    protected int OwnChatrge;
    protected int EnergySpend;
    protected static readonly int OrganicVolume = 50;
    protected static readonly int EnergyVolume = 30;
    protected static Map CurentMap;
    private bool _energyWasSet = false;

    public Vector2Int Position => CurrentPosition;

    protected virtual void Awake()
    {
        CurrentPosition.x = Mathf.FloorToInt(transform.position.x);
        CurrentPosition.y = Mathf.FloorToInt(transform.position.z);
        if (!CurentMap.AddToRegistry(this)) Kill();
    }

    public static void SetMap(Map newMap)
    {
        CurentMap = newMap;
    }

    public virtual void Kill()
    {
        CurentMap.RemoveFromMap(CurrentPosition);
        Destroy(gameObject);
    }

    public void setStartEnergy(int startEnergy)
    {
        if (!_energyWasSet)
        {
            OwnChatrge = startEnergy;
            _energyWasSet = true;
        }
    }
}
