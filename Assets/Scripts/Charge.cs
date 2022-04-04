using UnityEngine;

public class Charge : EnergyArray
{
    public Charge(int sizeX, int sizeY) : base(sizeX, sizeY)
    {

    }

    public static int EqualizationTo = 100;
    public static int EqualizationStep = 1;

    public void Equalization()
    {
        for (int x = 0; x < MapCreator.MapSixeX; x++)
            for (int y = 0; y < MapCreator.MapSixeY; y++)
            {
                if (MainData[x, y] > EqualizationTo) MainData[x, y] -= EqualizationStep;
                if (MainData[x, y] < EqualizationTo) MainData[x, y] += EqualizationStep;
            }
    }

    public int TakeCharge(Vector2Int position)
    {
        int boofer = Mathf.Clamp(MainData[position.x, position.y], 0, 20);
        MainData[position.x, position.y] -= boofer;
        return boofer;
    }

}
