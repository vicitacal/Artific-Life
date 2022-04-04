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
    public static int MapSixeX { get; private set; } = 100; //2800;
    public static int MapSixeY { get; private set; } = 100; //1400;
    public static GameObject[] CellsPrefubs { get; private set; }
    public static UnityEvent Tick = new UnityEvent();
    private static float _tickPeriod = 0.5f;
    private GameObject _gameField;
    private Map _map;
    private Coroutine _tickCorotine;

    private void Awake()
    {
        InitMap();
        CellsPrefubs = new GameObject[] {null, _MainCreature, _Antenna, _Leaf, _Root, _Wood};
        _tickCorotine = StartCoroutine(EventTick());
    }

    public void InitMap()
    {
        _gameField = transform.GetComponentInChildren<Map>().gameObject;
        _gameField.transform.localScale = new Vector3(MapSixeX, MapSixeY, 1);
        _gameField.transform.position = new Vector3(MapSixeX/2 - 0.5f, 0, MapSixeY/2 - 0.5f);
        _gameField.GetComponent<Renderer>().material.mainTextureScale = new Vector2(MapSixeX, MapSixeY);
        _map = _gameField.GetComponent<Map>();

        Creature.SetMap(_map);
    }

    public static void SetTickPeriod(float newTick)
    {
        _tickPeriod = newTick;
    }

    private IEnumerator EventTick() //По хорошему, перенести куда то в более подходящий класс
    {
        while (true)
        {
            Tick.Invoke();
            yield return new WaitForSeconds(_tickPeriod);
        }
    }

}
