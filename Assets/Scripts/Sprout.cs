using System.Collections.Generic;
using UnityEngine;

public class Sprout : Creature
{
    [SerializeField] private GameObject _tail;
    private MeshRenderer _head;
    private Genome _genome;
    private DirectionsDescript _currentDirection;
    private const int _sleepChargeSpend = 2;
    private const int _defaultChargeSpend = 20;
    [HideInInspector] public int ChildsCount = 0;
    private int _chargeSpendPerStep = 20;
    private int _chargeRize = 0;
    private int _previousEnergy = 0;
    private bool _isMultiCell = false;
    private bool _firstChld = false;
    private bool _prevOperationSuccess = false;
    private bool _sleeping = false;
    public int Charge => OwnCharge;
    public int ChargeChenge => _chargeRize;
    public Genome Genome => _genome;

    protected override void Awake()
    {
        base.Awake();
        _head = GetComponentInChildren<MeshRenderer>();
        _genome = new Genome(this, new List<Genome.Gene> {EatOrganic, Move, CreateChilds, ShiftOrganic, EatNeighbour, Sleep});
        MapCreator.Tick.AddListener(Tick);
        OwnCharge = 1500;
    }

    private void Tick()
    { 
        if (ChildsCount == 0)
        {
            ActivateTail(false);
            _sleeping = false;
            _chargeSpendPerStep = _defaultChargeSpend;
            _head.material.color = Color.white;
        }
        if (!_sleeping)
        {
            _prevOperationSuccess = _genome.PerformGene();
            _genome.MutateGenome(0.8f);
        }
        OwnCharge -= _chargeSpendPerStep;
        _chargeRize = OwnCharge - _previousEnergy;
        _previousEnergy = OwnCharge;
        if (OwnCharge <= 0) Kill();
    }
    
    private bool CreateChilds(Genome.Comand inputComand)
    {
        bool isSpawned = false;
        isSpawned |= CreateChild(inputComand.FirstChild);
        isSpawned |= CreateChild(inputComand.SecondChild);
        isSpawned |= CreateChild(inputComand.ThirdChild);

        if (isSpawned)
        {
            Move(_currentDirection, inputComand.getHighstChildCost());
            return true;
        }
        return false;
    }

    private bool Move(Genome.Comand inputComand)
    {
        if (!IsMoveAvaliable(inputComand.MoveDirection, CurrentPosition)) return false;
        if (!CurrentMap.ChengeObjectPosition(CurrentPosition, inputComand.MoveDirection.nextStepPosition(CurrentPosition))) return false;
        if (_isMultiCell)
        {
            Wood newWood = Instantiate(MapCreator.CellsPrefubs[5], new Vector3(transform.position.x, 0, transform.position.z), transform.rotation).GetComponent<Wood>();
            newWood.setStartEnergy(inputComand.getHighstChildCost());
        }
        CurrentPosition = inputComand.MoveDirection.nextStepPosition(CurrentPosition);
        _currentDirection = inputComand.MoveDirection;
        gameObject.transform.position = new Vector3(CurrentPosition.x, 0, CurrentPosition.y);
        gameObject.transform.rotation = CalculateRotation(_currentDirection.direction);
        return true;
    }

    private bool Move(DirectionsDescript inDirection, int stickCost)
    {
        Genome.Comand comand = new Genome.Comand();
        comand.MoveDirection = inDirection;
        comand.FirstChild.ChildCost = stickCost;
        return Move(comand);
    }

    private bool Sleep(Genome.Comand inputComand)
    {
        if (_isMultiCell)
        {
            _sleeping = true;
            _chargeSpendPerStep = _sleepChargeSpend;
            _head.material.color = new Color(0.52f, 0.12f, 0);
            return true;
        }
        return false;
    }

    private bool EatOrganic(Genome.Comand inputComand)
    {
        int resivedCharge = CurrentMap.OrganicField.TakeOrganic(CurrentPosition);
        if (resivedCharge > 0)
        {
            OwnCharge += resivedCharge;
            return true;
        }
        else 
            return false;
    }

    private bool ShiftOrganic(Genome.Comand inputComand)
    {
        return CurrentMap.OrganicField.ShiftValue(CurrentPosition, _currentDirection);
    }

    private bool EatNeighbour(Genome.Comand inputComand)
    {
        List<Creature> eatableObject = CurrentMap.GetEatableObjects(CurrentPosition, 2);
        while (eatableObject.Count > 0) {
            var eatIndex = Random.Range(0, eatableObject.Count);
            if (eatableObject[eatIndex].Parent == this)
            {
                eatableObject.RemoveAt(eatIndex);
            }
            else
            {
                OwnCharge += eatableObject[eatIndex].Eat();
                return true;
            }
        }
        return false;
    }

    private bool CreateChild(Genome.ChildDiscript childDiscript)
    {
        if (childDiscript.ChildType == 0 || !IsMoveAvaliable(_currentDirection, CurrentPosition)) return false;
        Vector2Int mapSpawnPos = childDiscript.Position.getChildCord(CurrentPosition, _currentDirection.direction);
        Vector3 worldSpawnPos = new Vector3(mapSpawnPos.x, 0, mapSpawnPos.y);
        Quaternion calculatedRotation = CalculateRotation(childDiscript.Position.getChildDirection(_currentDirection.direction));
        GameObject spawnetObject;
        Creature spawnedCreature;
        bool isCreated = false;

        if (!CurrentMap.IsPositionAvailable(mapSpawnPos)) return false;

        if (OwnCharge > childDiscript.ChildCost + _chargeSpendPerStep)
        {
            OwnCharge -= childDiscript.ChildCost;
            spawnetObject = Instantiate(MapCreator.CellsPrefubs[childDiscript.ChildType], worldSpawnPos, calculatedRotation);
            spawnedCreature = spawnetObject.GetComponent<Creature>();
            spawnedCreature.setStartEnergy(childDiscript.ChildCost);
            if (childDiscript.ChildType == 1)
            {
                spawnetObject.GetComponentInChildren<Sprout>().SetGenom(_genome.Genes);
            }
            else
            {
                spawnedCreature.Parent = this;
                ChildsCount++;
            }
            _firstChld = true;
            isCreated = true;
        }

        if (!_isMultiCell && _firstChld)
        {
            _tail.gameObject.SetActive(true);
            _isMultiCell = true;
        }
        return isCreated;
    }

    private Quaternion CalculateRotation(DirectionsDescript.Directions inDir)
    {
        var rotationSide = (int)inDir;
        rotationSide += 1;
        rotationSide %= 4;
        return Quaternion.Euler(new Vector3(0, rotationSide * 90, 0));
    }

    private bool IsMoveAvaliable(DirectionsDescript direction, Vector2Int curPos)
    {
        return CurrentMap.IsPositionAvailable(direction.nextStepPosition(curPos));
    }

    public bool CheckCondition(Genome.Comand inputComand)
    {
        int parametr = inputComand.ConditionArgument;

        switch (inputComand.Condition)
        {
            case 0:
                return OwnCharge > parametr;
            case 1:
                return OwnCharge <= parametr;
            case 3:
                return _isMultiCell;
            case 4:
                return _chargeRize > parametr / 10;
            case 5:
                return _chargeRize <= parametr / 10;
            case 6:
                return CurrentMap.IlluminationField.Values[CurrentPosition.x, CurrentPosition.y] > parametr / 50;
            case 7:
                return CurrentMap.IlluminationField.Values[CurrentPosition.x, CurrentPosition.y] <= parametr / 50;
            case 8:
                return CurrentMap.ChargeField.Values[CurrentPosition.x, CurrentPosition.y] > parametr / 50;
            case 9:
                return CurrentMap.ChargeField.Values[CurrentPosition.x, CurrentPosition.y] <= parametr / 50;
            case 10:
                return CurrentMap.OrganicField.Values[CurrentPosition.x, CurrentPosition.y] > parametr / 50;
            case 11:
                return CurrentMap.OrganicField.Values[CurrentPosition.x, CurrentPosition.y] <= parametr / 50;
            case 12:
                return _prevOperationSuccess;
            case 13:
                return IsMoveAvaliable(_currentDirection, CurrentPosition);
            case 14:
                return CurrentMap.GetEatableObjects(CurrentPosition, 2).Count > 0;
            default: 
                return true;
        }
    }

    public void SetGenom(Genome.Comand[] inputGenome)
    {
        _genome.SetGenom(inputGenome);
    }
    
    public void ReceiveEnergy(int energy)
    {
        OwnCharge += energy;
    }
    
    private void ActivateTail(bool active)
    {
        _isMultiCell = active;
        _tail.gameObject.SetActive(active);
    }

    public override void Kill()
    {
        CurrentMap.OrganicField.AddToArea(CurrentPosition, OrganicVolume / 9, 1);
        CurrentMap.ChargeField.AddToArea(CurrentPosition, ChargeVolume / 9, 1);
        base.Kill();
    }

}