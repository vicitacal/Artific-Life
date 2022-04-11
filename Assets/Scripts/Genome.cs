using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{
    public Genome(Sprout parent, List<Gene> genePool)
    {
        _parent = parent;
        _genePool = new List<Gene>(genePool);

        for (int j = 0; j < GenomeLenght; j++)
        {
            _genom[j] = Comand.getRandomComand();
        }
    }
    public Genome(Sprout parent, List<Gene> genePool, Comand[] comands)
    {
        _parent = parent;
        _genePool = new List<Gene>(genePool);
        comands.CopyTo(_genom, 0);
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
            return new Vector2Int(-1, -1);
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
                    switch (curDir)
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
        public int ChildCost;
        public ChildPlase Position;
    }
    public struct Comand
    {
        public byte ComandId;
        public DirectionsDescript MoveDirection;
        public byte Condition;
        public int ConditionArgument;
        public ChildDiscript FirstChild;
        public ChildDiscript SecondChild;
        public ChildDiscript ThirdChild;
        public byte Transition;

        public int getHighstChildCost()
        {
            var highstCost = FirstChild.ChildCost;
            if (SecondChild.ChildCost > highstCost) highstCost = SecondChild.ChildCost;
            if (ThirdChild.ChildCost > highstCost) highstCost = ThirdChild.ChildCost;
            return highstCost;
        }

        public static Comand getRandomComand()
        {
            Comand newRandomComand = new Comand();
            newRandomComand.ComandId = (byte)Random.Range(0, 6);
            newRandomComand.MoveDirection = new DirectionsDescript((byte)Random.Range(0, 4));
            newRandomComand.Condition = (byte)Random.Range(0, 16);
            newRandomComand.ConditionArgument = (byte)Random.Range(0, 500);
            newRandomComand.FirstChild = new ChildDiscript((byte)Random.Range(0, 5), 0, ChildPlase.AvaliableOrdinal.firstPos);
            newRandomComand.SecondChild = new ChildDiscript((byte)Random.Range(0, 5), 0, ChildPlase.AvaliableOrdinal.secondPos);
            newRandomComand.ThirdChild = new ChildDiscript((byte)Random.Range(0, 5), 0, ChildPlase.AvaliableOrdinal.thirdPos);
            newRandomComand.Transition = (byte)Random.Range(0, GenomeLenght);
            newRandomComand.FirstChild.ChildCost = newRandomComand.FirstChild.ChildType == 1 ? Random.Range(150, 450) : Random.Range(50, 350);
            newRandomComand.SecondChild.ChildCost = newRandomComand.SecondChild.ChildType == 1 ? Random.Range(150, 450) : Random.Range(50, 350);
            newRandomComand.ThirdChild.ChildCost = newRandomComand.ThirdChild.ChildType == 1 ? Random.Range(150, 450) : Random.Range(50, 350);
            return newRandomComand;
        }
    }

    public const int GenomeLenght = 24;
    public delegate bool Gene(Comand value);
    private int _performingOperationNum = 0;
    private Sprout _parent;
    private List<Gene> _genePool = new List<Gene>();
    private Comand[] _genom = new Comand[GenomeLenght];

    public Comand[] Genes => _genom;

    public void MutateGenome(float chance)
    {
        if (chance < Random.Range(0f, 1f)) return;
        var geneToChenge = Random.Range(0, GenomeLenght);
        switch (Random.Range(1, 9))
        {
            case 1:
                _genom[geneToChenge].ComandId = (byte)Random.Range(0, 6);
                break;
            case 2:
                _genom[geneToChenge].MoveDirection = new DirectionsDescript((byte)Random.Range(0, 4));
                break;
            case 3:
                _genom[geneToChenge].Condition = (byte)Random.Range(0, 16);
                break;
            case 4:
                _genom[geneToChenge].ConditionArgument = (byte)Random.Range(0, 500);
                break;
            case 5:
                switch (Random.Range(1, 3))
                {
                    case 1:
                        _genom[geneToChenge].FirstChild.ChildType = (byte)Random.Range(0, 5);
                        break;
                    case 2:
                        _genom[geneToChenge].FirstChild.ChildCost = _genom[geneToChenge].FirstChild.ChildType == 1 ? Random.Range(150, 450) : Random.Range(50, 350);
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
                        _genom[geneToChenge].SecondChild.ChildCost = _genom[geneToChenge].SecondChild.ChildType == 1 ? Random.Range(150, 450) : Random.Range(50, 350);
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
                        _genom[geneToChenge].ThirdChild.ChildCost = _genom[geneToChenge].ThirdChild.ChildType == 1 ? Random.Range(150, 450) : Random.Range(50, 350);
                        break;
                }
                break;
            case 8:
                _genom[geneToChenge].Transition = (byte)Random.Range(0, GenomeLenght);
                break;
        }
    }

    public void AddGenepool(List<Gene> genePool)
    {
        _genePool = new List<Gene>(genePool);
    }

    public bool PerformGene()
    {
        bool operationSucsess;
        Comand curentComand = _genom[_performingOperationNum];
        if (!_parent.CheckCondition(curentComand))
        {
            _performingOperationNum = curentComand.Transition;
            curentComand = _genom[_performingOperationNum];
        }

        operationSucsess = _genePool[curentComand.ComandId](curentComand);
        _performingOperationNum = curentComand.Transition;
        return operationSucsess;
    }

    public string GetDescription()
    {
        string description = "";

        for (int i = 0; i < _genom.Length; i++)
        {
            description += i == _performingOperationNum ? ">" : "  ";
            description += i + ") id:" + _genom[i].ComandId + " Dir:" + _genom[i].MoveDirection.direction + " Cond:" + _genom[i].Condition + " Arg:" + _genom[i].ConditionArgument;
            description += " C1:" + _genom[i].FirstChild.ChildType + " Cost:" + _genom[i].FirstChild.ChildCost + " C2:" + _genom[i].SecondChild.ChildType;
            description += " Cost:" + _genom[i].SecondChild.ChildCost + " C3:" + _genom[i].ThirdChild.ChildType + " Cost:" + _genom[i].ThirdChild.ChildCost;
            description += " Jump:" + _genom[i].Transition + "\n";
        }
        return description;
    }

    public void SetGenom(Comand[] inputGenome)
    {
        inputGenome.CopyTo(_genom, 0);
    }
}