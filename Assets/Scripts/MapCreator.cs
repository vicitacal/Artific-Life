using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private GameObject _MainCreature;
    [SerializeField] private GameObject _Antenna;
    [SerializeField] private GameObject _Leaf;
    [SerializeField] private GameObject _Root;
    [SerializeField] private GameObject _Wood;
    public static int MapSixeX { get; private set; } = 150;
    public static int MapSixeY { get; private set; } = 150;
    public static UnityEvent Tick = new UnityEvent();
    private GameObject _newObject;
    private GameObject _gameField;
    public static float StepLenght = 0.4f;
    public static GameObject[] CellsPrefubs { get; private set; }
    private Map _map;
    private Sprout.Comand[] _randomGenom = new Sprout.Comand[Sprout.GenomeLenght];
    private Coroutine _tickCorotine;
    private static float _tickPeriod = 0.5f;

    private void Awake()
    {
        CellsPrefubs = new GameObject[6];
        CellsPrefubs[0] = null;
        CellsPrefubs[1] = _MainCreature;
        CellsPrefubs[2] = _Antenna;
        CellsPrefubs[3] = _Leaf;
        CellsPrefubs[4] = _Root;
        CellsPrefubs[5] = _Wood;

        InitMap();
        _tickCorotine = StartCoroutine(EventTick());
    }

    public void InitMap()
    {
        _gameField = transform.GetComponentInChildren<Map>().gameObject;
        _gameField.transform.localScale = new Vector3(MapSixeX / 2.5f, MapSixeY / 2.5f, 1);
        _gameField.transform.position = new Vector3(MapSixeX / 5f, 0, MapSixeY / 5f);
        _gameField.GetComponent<Renderer>().material.mainTextureScale = new Vector2(MapSixeX, MapSixeY);
        _map = _gameField.GetComponent<Map>();

        Creature.SetMap(_map);
    }

    public void createSprouts(int quantity)
    {
        int curX = 3, curY = 3;

        for (int i = 0; i < quantity; i++)
        {
            _newObject = Instantiate(CellsPrefubs[1], new Vector3((curX * StepLenght) + StepLenght / 2, 0,( curY * StepLenght) + StepLenght / 2), Quaternion.identity);
            for (int j = 0; j < Sprout.GenomeLenght; j++)
            {
                _randomGenom[j] = Sprout.Comand.getRandomComand();
            }
            _newObject.GetComponentInChildren<Sprout>().SetGenom(_randomGenom);

            curX += 7;
            if (curX > MapSixeX - 1)
            {
                curX = 3;
                curY += 7;
            }
        }
    }

    public static void setTickPeriod(float newTick)
    {
        _tickPeriod = newTick;
    }

    private IEnumerator EventTick()
    {
        while (true)
        {
            Tick.Invoke();
            yield return new WaitForSeconds(_tickPeriod);
        }
    }

}
