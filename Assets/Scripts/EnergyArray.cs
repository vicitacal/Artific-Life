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
        for (int x = GeneralPurpose.CutToMapSizeX(posToAdd.x - radius); x <= GeneralPurpose.CutToMapSizeX(posToAdd.x + radius); x++)
            for (int y = GeneralPurpose.CutToMapSizeY(posToAdd.y - radius); y <= GeneralPurpose.CutToMapSizeY(posToAdd.y + radius); y++)
                MainData[x, y] += value;
    }

    public void Clear()
    {
        MainData = new int[Size.x,Size.y];
    }

}
