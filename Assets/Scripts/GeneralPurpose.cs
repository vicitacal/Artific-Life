using UnityEngine;

public static class GeneralPurpose
{
    public static int MapSizeX, MapSizeY;
    public static int CutToMapSizeX(int val)
    {
        //return (MapCreator.MapSixeX + val % MapCreator.MapSixeX) % MapCreator.MapSixeX;
        if (val >= MapSizeX) return val - (MapSizeX - 1);
        if (val < 0) return val + MapSizeX;
        return val;
    }
    public static int CutToMapSizeY(int val)
    {
        //return (MapCreator.MapSixeY + val % MapCreator.MapSixeY) % MapCreator.MapSixeY;
        if (val >= MapSizeY) return val - (MapSizeY - 1);
        if (val < 0) return val + MapSizeY;
        return val;
    }
}

[System.Serializable]
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
            case Directions.left: return new Vector2Int(GeneralPurpose.CutToMapSizeX(curPos.x - 1), curPos.y);
            case Directions.up: return new Vector2Int(curPos.x, GeneralPurpose.CutToMapSizeY(curPos.y + 1));
            case Directions.right: return new Vector2Int(GeneralPurpose.CutToMapSizeX(curPos.x + 1), curPos.y);
            case Directions.down: return new Vector2Int(curPos.x, GeneralPurpose.CutToMapSizeY(curPos.y - 1));
        }
        return curPos;
    }
}

public enum ViewModes
{
    NormalMode, IlluminationMode, OrganicMode, ChargeMode
}

[System.Serializable]
public struct GenomeValues
{
    public Genome.Comand[] Genom;
    public int PerformingOperationNum;

    public GenomeValues(Genome.Comand[] comands, int operationNum)
    {
        Genom = new Genome.Comand[comands.Length];
        comands.CopyTo(Genom, 0);
        PerformingOperationNum = operationNum;
    }

    public string GetJson()
    {
        return JsonUtility.ToJson(this, true);
    }
}
