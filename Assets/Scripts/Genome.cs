using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome
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
    public const int ChargeSpendPerStep = 8;
    public const int GenomeLenght = 24;

    private Comand[] _genom = new Comand[GenomeLenght];

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
                switch (Random.Range(1, 3))
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

    void BLAT()
    {
         pls
    }
}
