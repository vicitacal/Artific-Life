using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Gradient _organicGradient;
    [SerializeField] private Gradient _energyGradient;
    [SerializeField] private Gradient _illuminationGradient;
    public int[,] Charge = new int[MapCreator.MapSixeX, MapCreator.MapSixeY];
    public int[,] Organic = new int[MapCreator.MapSixeX, MapCreator.MapSixeY];
    public int[,] Illumination = new int[MapCreator.MapSixeX, MapCreator.MapSixeY];
    private Creature[,] _objectsOnMap = new Creature[MapCreator.MapSixeX, MapCreator.MapSixeY];
    private Texture2D _texture;
    private ViewModes _viewMod;

    private void Awake()
    {
        MapCreator.Tick.AddListener(Tick);
        _texture = new Texture2D(MapCreator.MapSixeX, MapCreator.MapSixeY);
        _texture.filterMode = FilterMode.Point;
        Material[] objectMaterial = gameObject.GetComponent<Renderer>().materials;
        if(objectMaterial.Length > 1) objectMaterial[1].mainTexture = _texture;
        SetViewMode(ViewModes.NormalMode);
        UpdateTexture();
    }

    public void InitMap()
    {
        for (int x = 0; x < MapCreator.MapSixeX; x++)
            for (int y = 0; y < MapCreator.MapSixeY; y++)
            {
                Illumination[x, y] = (int)(Mathf.Sin((float)x / MapCreator.MapSixeX * Mathf.PI) * Mathf.Sin((float)y / MapCreator.MapSixeY * Mathf.PI) * 50);
                Charge[x, y] = 10;
            }
        UpdateTexture();
    }

    private void Tick()
    {
        for (int x = 0; x < MapCreator.MapSixeX; x++)
            for (int y = 0; y < MapCreator.MapSixeY; y++)
            {
                if (Charge[x, y] > 100) Charge[x, y]--;
                if (Charge[x, y] < 100) Charge[x, y]++;
            }
        if(_viewMod != ViewModes.NormalMode) UpdateTexture();
    }

    public void SetViewMode(ViewModes mode)
    {
        _viewMod = mode;
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        Color color = Color.white;
        for (int x = 0; x < MapCreator.MapSixeX; x++)
            for (int y = 0; y < MapCreator.MapSixeY; y++)
            {
                switch (_viewMod)
                {
                    case ViewModes.IlluminationMode:
                        color = _illuminationGradient.Evaluate(Illumination[x, y] / 50f);
                        break;
                    case ViewModes.OrganicMode:
                        color = _organicGradient.Evaluate(Organic[x, y] / 500f);
                        break;
                    case ViewModes.ChargeMode:
                        color = _energyGradient.Evaluate(Charge[x, y] / 500f);
                        break;
                }
                _texture.SetPixel(x, y, color);
            }
        _texture.Apply();
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
                Organic[i, j] += value;
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
