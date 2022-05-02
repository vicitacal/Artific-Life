using UnityEngine;

public class Charge : EnergyArray
{
    public static int EqualizationTo = 100;
    public static float EqualizationStep = 1;
    private float _stepValue;

    public Charge(int sizeX, int sizeY) : base(sizeX, sizeY)
    {

    }

    public void Equalization()
    {
        _stepValue += EqualizationStep;
        if (_stepValue >= 1)
        {
            int intStep = Mathf.FloorToInt(_stepValue);
            _stepValue -= intStep;
            for (int x = 0; x < MapCreator.MapSixeX; x++)
                for (int y = 0; y < MapCreator.MapSixeY; y++)
                {
                    if (MainData[x, y] > EqualizationTo) MainData[x, y] -= intStep;
                    if (MainData[x, y] < EqualizationTo) MainData[x, y] += intStep;
                }
        }
    }

    public int TakeCharge(Vector2Int position)
    {
        int boofer = Mathf.Clamp(MainData[position.x, position.y], 0, 20);
        MainData[position.x, position.y] -= boofer;
        return boofer;
    }

}
