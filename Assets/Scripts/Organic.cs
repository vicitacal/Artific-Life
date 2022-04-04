using System.Security.Claims;
using UnityEngine;

public class Organic : EnergyArray
{
    public Organic(int sizeX, int sizeY) : base(sizeX, sizeY)
    {

    } 

    public bool ShiftValue(Vector2Int from, DirectionsDescript dir)
    {
        if (from.x < 0 || from.y < 0 || from.x > Size.x - 1 || from.y > Size.y - 1) return false;
        Vector2Int targetPos = dir.nextStepPosition(from);
        targetPos.x = Mathf.Clamp(targetPos.x, 0, Size.x - 1);
        targetPos.y = Mathf.Clamp(targetPos.y, 0, Size.y - 1);
        if (from != targetPos)
        {
            MainData[targetPos.x, targetPos.y] += MainData[from.x, from.y];
            MainData[from.x, from.y] = 0;
            return true;
        }
        return false;
    }

    public int TakeOrganic(Vector2Int position)
    {
        int boofer = Mathf.Clamp(MainData[position.x, position.y], 0, 20);
        MainData[position.x, position.y] -= boofer;
        return boofer;
    }

}
