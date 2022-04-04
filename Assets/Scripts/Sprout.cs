using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprout : Creature
{
    public List<MiningCells> Childs = new List<MiningCells>();
    public const int ChargeSpendPerStep = 25;

    [SerializeField] private GameObject _tail;
    private Genome _genome;
    private DirectionsDescript _currentDirection;
    private int _chargeRize = 0;
    private int _previousEnergy = 0;
    private bool _isMultiCell = false;
    private bool _firstChld = false;
    private bool _prevOperationSuccess = false;

    public int Charge => OwnCharge;
    public int ChargeChenge => _chargeRize;
    public Genome Genome => _genome;

    protected override void Awake()
    {
        base.Awake();
        _genome = new Genome(this, new List<Genome.Gene> { EatOrganic, Move, CreateChilds, ShiftOrganic, EatNeighbour });
        MapCreator.Tick.AddListener(Tick);
        OwnCharge = 1000;
    }

    private void Tick()
    {
        if (Childs != null)
            for (int i = 0; i < Childs.Count; i++)
            {
                if (Childs[i] == null)
                {
                    Childs.RemoveAt(i);
                    i--;
                }
                else
                    OwnCharge += Childs[i].collectEnergy();
            }
        _prevOperationSuccess = _genome.PerformGene();
        OwnCharge -= ChargeSpendPerStep;
        _chargeRize = OwnCharge - _previousEnergy;
        _previousEnergy = OwnCharge;
        if (OwnCharge <= 0) Kill();
        _genome.MutateGenome(0.8f);
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
            if (Childs.Contains((MiningCells)eatableObject[eatIndex]))
                eatableObject.RemoveAt(eatIndex);
            else
            {
                MiningCells CellToEat = (MiningCells)eatableObject[eatIndex];
                OwnCharge += CellToEat.Eat();
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
        GameObject spawnedCreature;
        bool isCreated = false;

        if (!CurrentMap.IsPositionAvailable(mapSpawnPos)) return false;

        if (OwnCharge > childDiscript.ChildCost + ChargeSpendPerStep)
        {
            OwnCharge -= childDiscript.ChildCost;
            if (childDiscript.ChildType == 1)
            {
                spawnedCreature = Instantiate(MapCreator.CellsPrefubs[1], worldSpawnPos, calculatedRotation);
                Sprout createdSprout = spawnedCreature.GetComponentInChildren<Sprout>();
                createdSprout.SetGenom(_genome.Genes);
                createdSprout.ActivateTail();
                createdSprout.Childs = Childs;
                spawnedCreature.GetComponent<Creature>().setStartEnergy(childDiscript.ChildCost + 50);
            } 
            else
            {
                spawnedCreature = Instantiate(MapCreator.CellsPrefubs[childDiscript.ChildType], worldSpawnPos, calculatedRotation);
                spawnedCreature.GetComponent<Creature>().CellType = childDiscript.ChildType;
                Childs.Add(spawnedCreature.GetComponentInChildren<MiningCells>());
                spawnedCreature.GetComponent<Creature>().setStartEnergy(childDiscript.ChildCost);
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
        Genome.Comand[] newGenome = new Genome.Comand[inputGenome.Length];
        System.Array.Copy(inputGenome, newGenome, inputGenome.Length);
        _genome = new Genome(this, new List<Genome.Gene> { EatOrganic, Move, CreateChilds, ShiftOrganic, EatNeighbour }, newGenome);
    }
    
    private void ActivateTail()
    {
        _isMultiCell = true;
        _tail.gameObject.SetActive(true);
    }

    public override void Kill()
    {

        CurrentMap.OrganicField.AddToArea(CurrentPosition, OrganicVolume / 9, 1);
        CurrentMap.ChargeField.AddToArea(CurrentPosition, ChargeVolume / 9, 1);

        Destroy(gameObject);

    }
    
}