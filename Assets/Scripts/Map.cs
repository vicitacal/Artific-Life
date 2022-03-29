using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int[,] Charge = new int[MapCreator.MapSixeX, MapCreator.MapSixeY];
    public int[,] Organic = new int[MapCreator.MapSixeX, MapCreator.MapSixeY];
    public int[,] Illumination = new int[MapCreator.MapSixeX, MapCreator.MapSixeY];
    private Creature[,] _objectsOnMap = new Creature[MapCreator.MapSixeX, MapCreator.MapSixeY];

    private void Awake()
    {
        for (int i = 0; i < MapCreator.MapSixeX; i++)
        {
            for (int j = 0; j < MapCreator.MapSixeY; j++)
            {
                //Расчитываем по центру самое яркое освещение и меньше к краям
                Illumination[i, j] = (int)(Mathf.Sin((float)i / MapCreator.MapSixeX * Mathf.PI) * Mathf.Sin((float)j / MapCreator.MapSixeY * Mathf.PI) * 25);
                //Illumination[i, j] = 20;
            }
        }
    }

    public bool IsPositionAvailable(Vector2Int posToTest)
    {
        if (posToTest.x < 0 || posToTest.y < 0 || posToTest.x > MapCreator.MapSixeX - 1 || posToTest.y > MapCreator.MapSixeY - 1)
            return false;
        if (_objectsOnMap[posToTest.x, posToTest.y] == null)
            return true;
        return false;
    }

    public bool AddToRegistry(Creature newCreature)
    {
        Vector2Int newCoordinats = newCreature.Position;
        if (_objectsOnMap[newCoordinats.x, newCoordinats.y] == null)
        {
            _objectsOnMap[newCoordinats.x, newCoordinats.y] = newCreature;
            return true;
        }
        return false;
    }

    public void RemoveFromMap(Vector2Int removePosition)
    {
        _objectsOnMap[removePosition.x, removePosition.y] = null;
    }
    
    public bool ChengeObjectPosition(Vector2Int objectPosition, Vector2Int newObjectPosition)
    {
        if (objectPosition.x < 0 || objectPosition.y < 0 || objectPosition.x > MapCreator.MapSixeX - 1 || objectPosition.y > MapCreator.MapSixeY - 1) return false;
        if (_objectsOnMap[objectPosition.x, objectPosition.y] == null) return false;
        if (!IsPositionAvailable(newObjectPosition)) return false;
        _objectsOnMap[newObjectPosition.x, newObjectPosition.y] = _objectsOnMap[objectPosition.x, objectPosition.y];
        _objectsOnMap[objectPosition.x, objectPosition.y] = null;
        return true;
    }

    public void AddOrganic3x3(Vector2Int posToAdd, int value)
    {
        var fromX = Mathf.Clamp(posToAdd.x - 1, 0, MapCreator.MapSixeX);
        var toX = Mathf.Clamp(posToAdd.x + 1, 0, MapCreator.MapSixeX);
        var fromY = Mathf.Clamp(posToAdd.y - 1, 0, MapCreator.MapSixeY);
        var toY = Mathf.Clamp(posToAdd.y + 1, 0, MapCreator.MapSixeY);
        for (int i = fromX; i < toX; i++)
            for (int j = fromY; j < toY; j++)
                Charge[i, j] += value;
    }

    public void AddEnergy3x3(Vector2Int position, int value)
    {
        var fromX = Mathf.Clamp(position.x - 1, 0, MapCreator.MapSixeX);
        var toX = Mathf.Clamp(position.x + 1, 0, MapCreator.MapSixeX);
        var fromY = Mathf.Clamp(position.y - 1, 0, MapCreator.MapSixeY);
        var toY = Mathf.Clamp(position.y + 1, 0, MapCreator.MapSixeY);
        for (int i = fromX; i < toX; i++)
            for (int j = fromY; j < toY; j++)
                Charge[i, j] += value;
    }

    public List<Creature> GetEatableObjects(Vector2Int position, int radius)
    {
        List<Creature> EatableObjects = new List<Creature>();
        var fromX = Mathf.Clamp(position.x - radius, 0, MapCreator.MapSixeX);
        var toX = Mathf.Clamp(position.x + radius, 0, MapCreator.MapSixeX);
        var fromY = Mathf.Clamp(position.y - radius, 0, MapCreator.MapSixeY);
        var toY = Mathf.Clamp(position.y + radius, 0, MapCreator.MapSixeY);
        for (int i = fromX; i < toX; i++)
            for (int j = fromY; j < toY; j++)
                if (_objectsOnMap[i, j]?.CellType > 1 && _objectsOnMap[i, j].CellType < 5)
                {
                    EatableObjects.Add(_objectsOnMap[i, j]);
                }
        return EatableObjects;
    }
    public void ClearMap()
    {
        Creature[] allObjects = FindObjectsOfType<Creature>();
        foreach (Creature creature in allObjects)
        {
            Destroy(creature.gameObject);
        }

        Charge = new int[MapCreator.MapSixeX, MapCreator.MapSixeY];
        Organic = new int[MapCreator.MapSixeX, MapCreator.MapSixeY];
        Illumination = new int[MapCreator.MapSixeX, MapCreator.MapSixeY];
        _objectsOnMap = new Creature[MapCreator.MapSixeX, MapCreator.MapSixeY];
        Awake();

    }

    /*
    public void AddOrganic3x3(Vector2Int posToAdd, int value)
    {
        if (posToAdd.x < 0 || posToAdd.y < 0 || posToAdd.x > MapCreator.MapSixeX - 1 || posToAdd.y > MapCreator.MapSixeY - 1)
            return;

        Organic[posToAdd.x, posToAdd.y] += value;

        if (posToAdd.x > 0)
        {
            Organic[posToAdd.x - 1, posToAdd.y] += value;

            if (posToAdd.y > 0)
                Organic[posToAdd.x - 1, posToAdd.y - 1] += value;

            if (posToAdd.y < MapCreator.MapSixeY - 1)
                Organic[posToAdd.x - 1, posToAdd.y + 1] += value;

        }
        if (posToAdd.x < MapCreator.MapSixeX - 1)
        {
            Organic[posToAdd.x + 1, posToAdd.y] += value;

            if (posToAdd.y < MapCreator.MapSixeY - 1)
                Organic[posToAdd.x + 1, posToAdd.y + 1] += value;


            if (posToAdd.y > 0)
                Organic[posToAdd.x + 1, posToAdd.y - 1] += value;

        }
        if (posToAdd.y < MapCreator.MapSixeY - 1)
            Organic[posToAdd.x, posToAdd.y + 1] += value;

        if (posToAdd.y > 0)
            Organic[posToAdd.x, posToAdd.y - 1] += value;
    }
    */
    /*
    public void AddEnergy3x3(Vector2Int posToAdd, int value)
    {
        if (posToAdd.x < 0 || posToAdd.y < 0 || posToAdd.x > MapCreator.MapSixeX - 1 || posToAdd.y > MapCreator.MapSixeY - 1)
            return;

        Charge[posToAdd.x, posToAdd.y] += value;

        if (posToAdd.x > 0)
        {
            Charge[posToAdd.x - 1, posToAdd.y] += value;

            if (posToAdd.y > 0)
                Charge[posToAdd.x - 1, posToAdd.y - 1] += value;
            
            if (posToAdd.y < MapCreator.MapSixeY - 1)
                Charge[posToAdd.x - 1, posToAdd.y + 1] += value;

        }
        if (posToAdd.x < MapCreator.MapSixeX - 1)
        {
            Charge[posToAdd.x + 1, posToAdd.y] += value;

            if (posToAdd.y < MapCreator.MapSixeY - 1)
                Charge[posToAdd.x + 1, posToAdd.y + 1] += value;
            

            if (posToAdd.y > 0)
                Charge[posToAdd.x + 1, posToAdd.y - 1] += value;

        }
        if (posToAdd.y < MapCreator.MapSixeY - 1)
            Charge[posToAdd.x, posToAdd.y + 1] += value;
        
        if (posToAdd.y > 0)
            Charge[posToAdd.x, posToAdd.y - 1] += value;
    }
    */

}
