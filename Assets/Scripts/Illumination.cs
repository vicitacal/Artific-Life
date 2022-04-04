using UnityEngine;

public class Illumination : EnergyArray
{
    public Illumination(int sizeX, int sizeY) : base(sizeX, sizeY)
    {

    }

    public void FillInCircle()
    {
        for (int x = 0; x < MapCreator.MapSixeX; x++)
            for (int y = 0; y < MapCreator.MapSixeY; y++)
            {
                MainData[x, y] = (int)(Mathf.Sin((float)x / MapCreator.MapSixeX * Mathf.PI) * Mathf.Sin((float)y / MapCreator.MapSixeY * Mathf.PI) * 50);
            }
    }

    public int pickValue(Vector2Int position)
    {
        return MainData[position.x, position.y];   
    }
}
