using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button _spawnButton;
    [SerializeField] private Text _spawnCountText;
    [SerializeField] private Slider _sliderSpawnCount;
    [SerializeField] private Map _map;
    [SerializeField] private RectTransform _buttonImageTransform;
    [SerializeField] private RectTransform _descriptionTransform;
    [SerializeField] private Slider _sliderTime;
    private bool _isRolled = true;

    private void Awake()
    {
        _sliderSpawnCount.maxValue = (MapCreator.MapSixeX * MapCreator.MapSixeY) / 100;
        _spawnCountText.text = "1";
    }
    public void ChangeCountText()
    {
        _spawnCountText.text = _sliderSpawnCount.value.ToString();
    }
    public void CallSpawner()
    {
        _map.CreateSprouts(Mathf.FloorToInt(_sliderSpawnCount.value));
        _spawnButton.interactable = false;
    }

    public void RollDescript()
    {
        if (_isRolled)
        {
            _descriptionTransform.offsetMin = new Vector2(0f, 445f);
            _buttonImageTransform.rotation = Quaternion.Euler(0f, 0f, 180f);
            _isRolled = false;
        }
        else
        {
            _descriptionTransform.offsetMin = new Vector2(0f, 0f);
            _buttonImageTransform.rotation = Quaternion.identity;
            _isRolled = true;
        }
    }
    public void ChengeTickPeriod()
    {
        MapCreator.SetTickPeriod(_sliderTime.value);
    }

    public void ClearScene()
    {
        FindObjectOfType<Map>().ClearMap();
        _spawnButton.interactable = true;
    }

}
