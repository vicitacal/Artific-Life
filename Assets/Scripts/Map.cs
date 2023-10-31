using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Gradient _organicGradient;
    [SerializeField] private Gradient _energyGradient;
    [SerializeField] private Gradient _illuminationGradient;
    public Charge ChargeField { get; private set; }
    public Organic OrganicField { get; private set; }
    public Illumination IlluminationField { get; private set; }
    private Creature[,] _objectsOnMap = new Creature[MapCreator.MapSixeX, MapCreator.MapSixeY];
    private int _objectsOnMapCount = 0;
    private Texture2D _texture;
    private ViewModes _viewMod;
    private bool _isSpawned = false;

    private void Awake()
    {
        InitArrays();
        InitTexture();
        SetViewMode(ViewModes.NormalMode);
        UpdateTexture();
        MapCreator.Tick.AddListener(Tick);
    }

    public void InitMap()
    {
        ClearArrays();
        IlluminationField.FillInCircle();
        UpdateTexture();
    }

    private void Tick()
    {
        ChargeField.Equalization();
        if (_viewMod != ViewModes.NormalMode) UpdateTexture();
        if (_isSpawned && _objectsOnMapCount == 0) UIController.Instance.ClearScene();
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
        _isSpawned = true;
    }

    public void SetViewMode(ViewModes mode)
    {
        _viewMod = mode;
        UpdateTexture();
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
            _objectsOnMapCount++;
            return true;
        }
        return false;
    }
    
    public void RemoveFromMap(Vector2Int removePosition)
    {
        _objectsOnMap[removePosition.x, removePosition.y] = null;
        _objectsOnMapCount--;
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
    
    public List<Creature> GetEatableObjects(Vector2Int position, int radius)
    {
        List<Creature> EatableObjects = new List<Creature>();
        for (int i = position.x - radius; i < position.x + radius; i++)
            for (int j = position.y - radius; j < position.y + radius; j++)
            {
                int x = GeneralPurpose.CutToMapSizeX(i);
                int y = GeneralPurpose.CutToMapSizeY(j);
                if (_objectsOnMap[x, y] != null && _objectsOnMap[x, y].CellType < 5 && i != position.x && j != position.y)
                {
                    EatableObjects.Add(_objectsOnMap[x, y]);
                }
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
        _objectsOnMapCount = 0;
        _isSpawned = false;
    }

    private void ClearArrays()
    {
        _objectsOnMap = new Creature[MapCreator.MapSixeX, MapCreator.MapSixeY];
        ChargeField.Clear();
        OrganicField.Clear();
        IlluminationField.Clear();
    }

    public void InitArrays()
    {
        ChargeField = new Charge(MapCreator.MapSixeX, MapCreator.MapSixeY);
        OrganicField = new Organic(MapCreator.MapSixeX, MapCreator.MapSixeY);
        IlluminationField = new Illumination(MapCreator.MapSixeX, MapCreator.MapSixeY);
        _objectsOnMap = new Creature[MapCreator.MapSixeX, MapCreator.MapSixeY];
    }
    
    public void InitTexture()
    {
        _texture = new Texture2D(MapCreator.MapSixeX, MapCreator.MapSixeY);
        _texture.filterMode = FilterMode.Point;
        Material[] objectMaterial = gameObject.GetComponent<Renderer>().materials;
        if (objectMaterial.Length > 1) objectMaterial[1].mainTexture = _texture;
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
                        color = _illuminationGradient.Evaluate(IlluminationField.Values[x, y] / (float)Illumination.Multiplayer);
                        break;
                    case ViewModes.OrganicMode:
                        color = _organicGradient.Evaluate(OrganicField.Values[x, y] / (float)Creature.OrganicDamageTreshold);
                        break;
                    case ViewModes.ChargeMode:
                        color = _energyGradient.Evaluate(ChargeField.Values[x, y] / (float)Creature.EnergyDamageTreshold);
                        break;
                }
                _texture.SetPixel(x, y, color);
            }
        _texture.Apply();
    }
}
