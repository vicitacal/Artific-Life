using UnityEngine;

public abstract class EnergyArray
{
    protected EnergyArray(int sizeX, int sizeY)
    {
        Size.Set(sizeX, sizeY);
        MainData = new int[sizeX, sizeY];
    }

    protected int[,] MainData;
    protected Vector2Int Size;
    public int[,] Values => MainData;

    public void AddToArea(Vector2Int posToAdd, int value, int radius)
    {
        var fromX = Mathf.Clamp(posToAdd.x - radius, 0, Size.x);
        var toX = Mathf.Clamp(posToAdd.x + radius, 0, Size.x);
        var fromY = Mathf.Clamp(posToAdd.y - radius, 0, Size.y);
        var toY = Mathf.Clamp(posToAdd.y + radius, 0, Size.y);
        for (int x = fromX; x < toX; x++)
            for (int y = fromY; y < toY; y++)
                MainData[x, y] += value;
    }

    public void Clear()
    {
        MainData = new int[Size.x,Size.y];
    }

}
