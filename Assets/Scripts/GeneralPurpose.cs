using UnityEngine;

public static class GeneralPurpose
{
    public static int CutToMapSize(int val)
    {
        return (MapCreator.MapSixeX + val % MapCreator.MapSixeX) % MapCreator.MapSixeX;
    }
}

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
            case Directions.left: return new Vector2Int(GeneralPurpose.CutToMapSize(curPos.x - 1), curPos.y);
            case Directions.up: return new Vector2Int(curPos.x, GeneralPurpose.CutToMapSize(curPos.y + 1));
            case Directions.right: return new Vector2Int(GeneralPurpose.CutToMapSize(curPos.x + 1), curPos.y);
            case Directions.down: return new Vector2Int(curPos.x, GeneralPurpose.CutToMapSize(curPos.y - 1));
        }
        return curPos;
    }
}

public enum ViewModes
{
    NormalMode, IlluminationMode, OrganicMode, ChargeMode
}
