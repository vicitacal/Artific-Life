using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprout : Creature
{
    public struct DirectionsDescript
    {
        public DirectionsDescript(byte dir)
        {
            direction = (Directions)dir;
        }
        public enum Directions
        {
            left, up, right, down
        }
        public Directions direction;
        public Vector2Int nextStepPosition(Vector2Int curPos)
        {
            switch (direction)
            {
                case Directions.left: return new Vector2Int(curPos.x - 1, curPos.y);
                case Directions.up: return new Vector2Int(curPos.x, curPos.y + 1);
                case Directions.right: return new Vector2Int(curPos.x + 1, curPos.y);
                case Directions.down: return new Vector2Int(curPos.x, curPos.y - 1);
            }
            return curPos;
        }
    }
    public struct ChildPlase
    {
        public enum AvaliableOrdinal
        {
            firstPos, secondPos, thirdPos
        }
        public AvaliableOrdinal Ordinal;
        public Vector2Int getChildCord(Vector2Int CurPos, DirectionsDescript.Directions curDir)
        {
            switch (Ordinal)
            {
                case AvaliableOrdinal.firstPos:
                    switch (curDir)
                    {
                        case DirectionsDescript.Directions.left:
                            return new Vector2Int(CurPos.x, CurPos.y - 1);
                        case DirectionsDescript.Directions.up:
                            return new Vector2Int(CurPos.x - 1, CurPos.y);
                        case DirectionsDescript.Directions.right:
                            return new Vector2Int(CurPos.x, CurPos.y + 1);
                        case DirectionsDescript.Directions.down:
                            return new Vector2Int(CurPos.x + 1, CurPos.y);
                    }
                    break;
                case AvaliableOrdinal.secondPos:
                    switch (curDir)
                    {
                        case DirectionsDescript.Directions.left:
                            return new Vector2Int(CurPos.x + 1, CurPos.y);
                        case DirectionsDescript.Directions.up:
                            return new Vector2Int(CurPos.x, CurPos.y - 1);
                        case DirectionsDescript.Directions.right:
                            return new Vector2Int(CurPos.x - 1, CurPos.y);
                        case DirectionsDescript.Directions.down:
                            return new Vector2Int(CurPos.x, CurPos.y + 1);
                    }
                    break;
                case AvaliableOrdinal.thirdPos:
                    switch (curDir)
                    {
                        case DirectionsDescript.Directions.left:
                            return new Vector2Int(CurPos.x, CurPos.y + 1);
                        case DirectionsDescript.Directions.up:
                            return new Vector2Int(CurPos.x + 1, CurPos.y);
                        case DirectionsDescript.Directions.right:
                            return new Vector2Int(CurPos.x, CurPos.y - 1);
                        case DirectionsDescript.Directions.down:
                            return new Vector2Int(CurPos.x - 1, CurPos.y);
                    }
                    break;
            }
            return new Vector2Int(-1,-1);
        }

        public DirectionsDescript.Directions getChildDirection(DirectionsDescript.Directions curDir)
        {
            switch (Ordinal)
            {
                case AvaliableOrdinal.firstPos:
                    switch (curDir)
                    {
                        case DirectionsDescript.Directions.left:
                            return DirectionsDescript.Directions.down;
                        case DirectionsDescript.Directions.up:
                            return DirectionsDescript.Directions.left;
                        case DirectionsDescript.Directions.right:
                            return DirectionsDescript.Directions.up;
                        case DirectionsDescript.Directions.down:
                            return DirectionsDescript.Directions.right;
                    }
                    break;
                case AvaliableOrdinal.secondPos:
                    switch (curDir)
                    {
                        case DirectionsDescript.Directions.left:
                            return DirectionsDescript.Directions.right;
                        case DirectionsDescript.Directions.up:
                            return DirectionsDescript.Directions.down;
                        case DirectionsDescript.Directions.right:
                            return DirectionsDescript.Directions.left;
                        case DirectionsDescript.Directions.down:
                            return DirectionsDescript.Directions.up;
                    }
                    break;
                case AvaliableOrdinal.thirdPos:
                    switch(curDir)
                    {
                        case DirectionsDescript.Directions.left:
                            return DirectionsDescript.Directions.up;
                        case DirectionsDescript.Directions.up:
                            return DirectionsDescript.Directions.right;
                        case DirectionsDescript.Directions.right:
                            return DirectionsDescript.Directions.down;
                        case DirectionsDescript.Directions.down:
                            return DirectionsDescript.Directions.left;
                    }
                    break;
                    
            }
            return DirectionsDescript.Directions.left;
        }
    }
    public struct ChildDiscript
    {
        public ChildDiscript(byte type, byte cost, ChildPlase.AvaliableOrdinal pos)
        {
            ChildType = type;
            ChildCost = cost;
            Position.Ordinal = pos;
        }
        public byte ChildType;
        public byte ChildCost;
        public ChildPlase Position;
    }
    public struct Comand
    {
        public byte ComandId;
        public byte MoveDirection;
        public byte Condition;
        public byte ConditionArgument;
        public ChildDiscript FirstChild;
        public ChildDiscript SecondChild;
        public ChildDiscript ThirdChild;
        public byte Transition;

        public int getHighstCost()
        {
            var highstCost = FirstChild.ChildCost;
            if (SecondChild.ChildCost > highstCost) highstCost = SecondChild.ChildCost;
            if (ThirdChild.ChildCost > highstCost) highstCost = ThirdChild.ChildCost;
            return highstCost;
        }

        public static Comand getRandomComand()
        {
            Comand newRandomComand = new Comand();
            newRandomComand.ComandId = (byte)Random.Range(0, 5);
            newRandomComand.MoveDirection = (byte)Random.Range(0, 4);
            newRandomComand.Condition = (byte)Random.Range(0, 16);
            newRandomComand.ConditionArgument = (byte)Random.Range(0, 200);
            newRandomComand.FirstChild = new ChildDiscript((byte)Random.Range(0, 5), (byte)Random.Range(30, 101), ChildPlase.AvaliableOrdinal.firstPos);
            newRandomComand.SecondChild = new ChildDiscript((byte)Random.Range(0, 5), (byte)Random.Range(30, 101), ChildPlase.AvaliableOrdinal.secondPos);
            newRandomComand.ThirdChild = new ChildDiscript((byte)Random.Range(0, 5), (byte)Random.Range(30, 101), ChildPlase.AvaliableOrdinal.thirdPos);
            newRandomComand.Transition = (byte)Random.Range(0, Sprout.GenomeLenght);
            return newRandomComand;
        }
    }

    public delegate bool Gene(Comand value);
    public List<Gene> GenePool = new List<Gene>();
    public List<MiningCells> Childs = new List<MiningCells>();
    public const int ChargeSpendPerStep = 8;
    public const int GenomeLenght = 24;

    [SerializeField] private GameObject _tail;
    private Comand[] _genom = new Comand[GenomeLenght];
    private DirectionsDescript _currentDirection;
    private int _performingOperationNum = 0;
    private int _chargeRize = 0;
    private int _previousEnergy = 0;
    private bool _isMultiCell = false;
    private bool _firstChld = false;
    private bool _prevOperationSuccess = false;

    public int Charge => OwnChatrge;
    public Comand[] Genom => _genom;
    public int ChargeChenge => _chargeRize;
    public int Opreation => _performingOperationNum;

    protected override void Awake()
    {
        base.Awake();
        GenePool.Add(EatOrganic);
        GenePool.Add(MoveSprout);
        GenePool.Add(CreateChilds);
        GenePool.Add(ShiftOrganic);
        GenePool.Add(EatNeighbour);
        MapCreator.Tick.AddListener(sproutTick);
        OwnChatrge = 300;
    }

    private void sproutTick()
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
                    OwnChatrge += Childs[i].collectEnergy();
            }

        Comand curentComand = _genom[_performingOperationNum];
        if (!checkCondition(curentComand))
        {
            _performingOperationNum = curentComand.Transition;
            curentComand = _genom[_performingOperationNum];
        }   
        
        _prevOperationSuccess = GenePool[curentComand.ComandId](curentComand);
        _performingOperationNum = curentComand.Transition;
        
        OwnChatrge -= ChargeSpendPerStep;
        _chargeRize = OwnChatrge - _previousEnergy;
        _previousEnergy = OwnChatrge;

        if (OwnChatrge <= 0) Kill();

        mutateGenome(0.8f);
    }

    public bool MoveSprout(DirectionsDescript inDirection)
    {
        if (!IsMoveAvaliable(inDirection, CurrentPosition)) return false;
        if (!CurentMap.ChengeObjectPosition(CurrentPosition, inDirection.nextStepPosition(CurrentPosition))) return false;
        if (_isMultiCell)
        {
            Wood newWood = Instantiate(MapCreator.CellsPrefubs[5], new Vector3(transform.position.x, 0, transform.position.z), transform.rotation).GetComponent<Wood>();
            newWood.setStartEnergy(_genom[_performingOperationNum].getHighstCost());
        }
        CurrentPosition = inDirection.nextStepPosition(CurrentPosition);
        _currentDirection = inDirection;
        gameObject.transform.position = calculateWorldtPosition(CurrentPosition);
        gameObject.transform.rotation = calculateRotation(_currentDirection.direction);
        return true;
    }

    public bool MoveSprout(Comand inputComand)
    {
        if (inputComand.MoveDirection >= 0 && inputComand.MoveDirection < 4)
            return MoveSprout(new DirectionsDescript(inputComand.MoveDirection));
        return false;
    }

    private void mutateGenome(float chance)
    {
        if (chance < Random.Range(0f, 1f)) return;
        var geneToChenge = Random.Range(0, GenomeLenght);
        switch (Random.Range(1, 9))
        {
            case 1:
                _genom[geneToChenge].ComandId = (byte)Random.Range(0, 3);
                break;
            case 2:
                _genom[geneToChenge].MoveDirection = (byte)Random.Range(0, 4);
                break;
            case 3:
                _genom[geneToChenge].Condition = (byte)Random.Range(0, 33);
                break;
            case 4:
                _genom[geneToChenge].ConditionArgument = (byte)Random.Range(0, 51);
                break;
            case 5:
                switch(Random.Range(1, 3))
                {
                    case 1:
                        _genom[geneToChenge].FirstChild.ChildType = (byte)Random.Range(0, 5);
                        break;
                    case 2:
                        _genom[geneToChenge].FirstChild.ChildCost = (byte)Random.Range(12, 61);
                        break;
                }
                break;
            case 6:
                switch (Random.Range(1, 3))
                {
                    case 1:
                        _genom[geneToChenge].SecondChild.ChildType = (byte)Random.Range(0, 5);
                        break;
                    case 2:
                        _genom[geneToChenge].SecondChild.ChildCost = (byte)Random.Range(12, 61);
                        break;
                }
                break;
            case 7:
                switch (Random.Range(1, 3))
                {
                    case 1:
                        _genom[geneToChenge].ThirdChild.ChildType = (byte)Random.Range(0, 5);
                        break;
                    case 2:
                        _genom[geneToChenge].ThirdChild.ChildCost = (byte)Random.Range(12, 61);
                        break;
                }
                break;
            case 8:
                _genom[geneToChenge].Transition = (byte)Random.Range(0, Sprout.GenomeLenght);
                break;
        }
    }

    private Vector3 calculateWorldtPosition(int posX, int posY, float posZ = 0)
    {
        return new Vector3(posX * MapCreator.StepLenght + MapCreator.StepLenght / 2, posZ, posY * MapCreator.StepLenght + MapCreator.StepLenght / 2);
    }

    private Vector3 calculateWorldtPosition(Vector2Int inPos)
    {
        return calculateWorldtPosition(inPos.x, inPos.y);
    }

    private Quaternion calculateRotation(DirectionsDescript.Directions inDir)
    {
        var rotationSide = (int)inDir;
        rotationSide += 1;
        rotationSide %= 4;
        return Quaternion.Euler(new Vector3(0, rotationSide * 90, 0));
    }

    private bool IsMoveAvaliable(DirectionsDescript direction, Vector2Int curPos)
    {
        return CurentMap.IsPositionAvailable(direction.nextStepPosition(curPos));
    }

    public override void Kill()
    {

        CurentMap.AddOrganic3x3(CurrentPosition, OrganicVolume);
        CurentMap.AddEnergy3x3(CurrentPosition, OwnChatrge / 9);

        Destroy(gameObject);

    }

    public bool CreateChilds(Comand inputComand)
    {
        bool isSpawned = false;
        isSpawned |= createChild(inputComand.FirstChild);
        isSpawned |= createChild(inputComand.SecondChild);
        isSpawned |= createChild(inputComand.ThirdChild);
        if (isSpawned)
        {
            MoveSprout(_currentDirection);
            return true;
        }
        return false;
    }
    
    private bool createChild(ChildDiscript childDiscript)
    {
        if (childDiscript.ChildType == 0 || !IsMoveAvaliable(_currentDirection, CurrentPosition)) return false;
        Vector2Int mapSpawnPos = childDiscript.Position.getChildCord(CurrentPosition, _currentDirection.direction);
        Vector3 worldSpawnPos = calculateWorldtPosition(mapSpawnPos);
        Quaternion calculatedRotation = calculateRotation(childDiscript.Position.getChildDirection(_currentDirection.direction));
        GameObject spawnedCreature;
        bool isCreated = false;

        if (!CurentMap.IsPositionAvailable(mapSpawnPos)) return false;

        if (OwnChatrge > childDiscript.ChildCost + ChargeSpendPerStep)
        {
            OwnChatrge -= childDiscript.ChildCost;
            if (childDiscript.ChildType == 1)
            {
                spawnedCreature = Instantiate(gameObject, worldSpawnPos, calculatedRotation);
                Sprout createdSprout = spawnedCreature.GetComponentInChildren<Sprout>();
                createdSprout.SetGenom(_genom);
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
            _isMultiCell = true;
            _tail.gameObject.SetActive(true);
        }
        return isCreated;
    }

    public bool EatOrganic(Comand inputComand)
    {
        if (CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] >= 10)
        {
            OwnChatrge += 10;
            CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] -= 10;
            return true;
        }
        else if (CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] > 0)
        {
            OwnChatrge += CurentMap.Organic[CurrentPosition.x, CurrentPosition.y];
            CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] = 0;
            return true;
        }
        return false;
    }

    public bool ShiftOrganic(Comand inputComand)
    {
        if (CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] == 0) return false;
        var shiftPosition = new DirectionsDescript(inputComand.MoveDirection).nextStepPosition(CurrentPosition);
        CurentMap.Organic[shiftPosition.x, shiftPosition.y] += CurentMap.Organic[CurrentPosition.x, CurrentPosition.y];
        CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] = 0;
        return true;
    }

    public bool EatNeighbour(Comand inputComand)
    {
        List<Creature> eatableObject = CurentMap.GetEatableObjects(CurrentPosition, 2);
        while (eatableObject.Count > 0) {
            var eatIndex = Random.Range(0, eatableObject.Count);
            if (Childs.Contains((MiningCells)eatableObject[eatIndex]))
                eatableObject.RemoveAt(eatIndex);
            else
            {
                MiningCells CellToEat = (MiningCells)eatableObject[eatIndex];
                OwnChatrge += CellToEat.Eat();
                return true;
            }
        }
        return false;
    }
    private bool checkCondition(Comand inputComand)
    {
        byte parametr = inputComand.ConditionArgument;

        switch (inputComand.Condition)
        {
            case 0:
                return OwnChatrge > parametr;
            case 1:
                return OwnChatrge <= parametr;
            case 3:
                return _isMultiCell;
            case 4:
                return _chargeRize > parametr;
            case 5:
                return _chargeRize <= parametr;
            case 6:
                return CurentMap.Illumination[CurrentPosition.x, CurrentPosition.y] > parametr;
            case 7:
                return CurentMap.Illumination[CurrentPosition.x, CurrentPosition.y] <= parametr;
            case 8:
                return CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] > parametr;
            case 9:
                return CurentMap.Charge[CurrentPosition.x, CurrentPosition.y] <= parametr;
            case 10:
                return CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] > parametr;
            case 11:
                return CurentMap.Organic[CurrentPosition.x, CurrentPosition.y] <= parametr;
            case 12:
                return _prevOperationSuccess;
            case 13:
                return IsMoveAvaliable(_currentDirection, CurrentPosition);
            case 14:
                return CurentMap.GetEatableObjects(CurrentPosition, 2).Count > 0;
            default: 
                return true;
        }
    }

    public void ReciveEnergy(int value)
    {
        if (value > 0)
            OwnChatrge += value;
    }

    public void SetGenom(Comand[] inputGenome)
    {
        for (int i = 0; i < _genom.Length; i++)
        {
            _genom[i].ComandId = inputGenome[i].ComandId;
            _genom[i].MoveDirection = inputGenome[i].MoveDirection;
            _genom[i].Condition = inputGenome[i].Condition;
            _genom[i].ConditionArgument = inputGenome[i].ConditionArgument;
            _genom[i].FirstChild = inputGenome[i].FirstChild;
            _genom[i].SecondChild = inputGenome[i].SecondChild;
            _genom[i].ThirdChild = inputGenome[i].ThirdChild;
            _genom[i].Transition = inputGenome[i].Transition;
        }
    }

    public void ActivateTail()
    {
        _isMultiCell = true;
        _tail.gameObject.SetActive(true);
    }
}