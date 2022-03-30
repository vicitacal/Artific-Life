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
            for (int j = 0; j < MapCreator.MapSixeY; j++)
            {
                Illumination[i, j] = (int)(Mathf.Sin((float)i / MapCreator.MapSixeX * Mathf.PI) * Mathf.Sin((float)j / MapCreator.MapSixeY * Mathf.PI) * 50);
                Charge[i, j] = 10;
            }
        MapCreator.Tick.AddListener(Tick);
    }

    private void Tick()
    {
        for (int i = 0; i < MapCreator.MapSixeX; i++)
            for (int j = 0; j < MapCreator.MapSixeY; j++)
            {
                if (Charge[i, j] > 100) Charge[i, j]--;
                if (Charge[i, j] < 100) Charge[i, j]++;
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

    public void CreateSprouts(int quantity)
    {
        float aspectRatio = (float)MapCreator.MapSixeX / (float)MapCreator.MapSixeY;
        int countX = Mathf.CeilToInt(Mathf.Sqrt(quantity * aspectRatio));
        int countY = Mathf.CeilToInt((float)quantity / (float)countX);
        int ofsetX = MapCreator.MapSixeX / countX;
        int ofsetY = MapCreator.MapSixeY / countY;
        int curX = ofsetX / 2;
        int curY = ofsetY / 2;

        for (int i = 0; i < quantity; i++)
        {
            Instantiate(MapCreator.CellsPrefubs[1], new Vector3(curX, 0, curY), Quaternion.identity);

            curX += ofsetX;
            if (curX > MapCreator.MapSixeX - 1)
            {
                curX = ofsetX / 2;
                curY += ofsetY;
            }
        }
    }

}
