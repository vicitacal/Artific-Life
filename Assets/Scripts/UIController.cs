using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button _spawnButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Image _pauseButtonImage;
    [SerializeField] private Text _spawnCountText;
    [SerializeField] private Slider _sliderSpawnCount;
    [SerializeField] private Map _map;
    [SerializeField] private RectTransform _buttonImageTransform;
    [SerializeField] private RectTransform _descriptionTransform;
    [SerializeField] private Slider _sliderTime;
    [SerializeField] private Dropdown _viewModeSwicher;
    [SerializeField] private Text _statusText;
    [SerializeField] private GameObject _UiGenomTextFeeld;
    [SerializeField] private Sprite _playButtonTexture;
    [SerializeField] private Sprite _pauseButtonTexture;
    private bool _isRolled = true;
    private Sprout _targetSprout;
    private bool _isSproutSelected;
    public static UIController Instance;

    private void Awake()
    {
        Instance = this;
        _spawnCountText.text = "1";
    }

    private void OnEnable()
    {
        _sliderSpawnCount.maxValue = (MapCreator.MapSixeX * MapCreator.MapSixeY) / 100;
    }

    private void Update()
    {
        if (_isSproutSelected)
        {
            if (_targetSprout)
                printSproutDiscript(_statusText, _targetSprout);
            else
            {
                _isSproutSelected = false;
                _UiGenomTextFeeld.SetActive(false);    
            }     
        } 
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CameraControl.Instance.enabled = false;
            EscController.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void Pause()
    {
        if (MapCreator.Instance.PauseGame())
            _pauseButtonImage.overrideSprite = _playButtonTexture;
        else
            _pauseButtonImage.overrideSprite = _pauseButtonTexture;
    }
    public void ChangeCountText()
    {
        _spawnCountText.text = _sliderSpawnCount.value.ToString();
    }
    
    public void CallSpawner()
    {
        _map.InitMap();
        _map.CreateSprouts(Mathf.FloorToInt(_sliderSpawnCount.value));
        _spawnButton.interactable = false;
    }

    public void RollDescript()
    {
        if (_isRolled)
        { 
            _descriptionTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
            _buttonImageTransform.rotation = Quaternion.Euler(0f, 0f, 180f);
            _isRolled = false;
        }
        else
        {
            _descriptionTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 788.734f);
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
        _map.ClearMap();
        _spawnButton.interactable = true;
    }

    public void SwichViewMode()
    {
        _map.SetViewMode((ViewModes)_viewModeSwicher.value);
    }
    private void printSproutDiscript(Text text, Sprout sprout)
    {
        text.text = "";
        text.text += sprout.Genome.GetDescription();
        text.text += "-----------\n";
        text.text += "Charge: " + sprout.Charge + "\n";
        text.text += "Charge change: " + sprout.ChargeChenge;
    }

    public void ShowSproutDescript(Sprout target)
    {
        _isSproutSelected = target;
        _targetSprout = target;
        _UiGenomTextFeeld.SetActive(_isSproutSelected);
    }

}
