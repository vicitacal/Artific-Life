using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections.Generic;


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
    [SerializeField] private InputField _saveName;
    [SerializeField] private Dropdown _loadFilesDropdown;
    private List<String> _fileNames = new List<string>();
    private const string _myExtension = ".sprt";
    private string _baseSavePath;
    private bool _isRolled = true;
    private bool _isSproutSelected = false;
    private bool _fullDesctipt = false;
    private Sprout _targetSprout;
    public static UIController Instance;

    private void Awake()
    {
        Instance = this;
        _baseSavePath = Path.Combine(Application.dataPath, "Saves");
        Directory.CreateDirectory(_baseSavePath);
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
            Pause(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CameraControl.Instance.enabled = false;
            EscController.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void Pause(bool pause)
    {
        MapCreator.Instance.PauseGame(pause);
        if (pause)
            _pauseButtonImage.overrideSprite = _playButtonTexture;
        else
            _pauseButtonImage.overrideSprite = _pauseButtonTexture;
    }
    public void ChangeCountText()
    {
        _spawnCountText.text = _sliderSpawnCount.value.ToString();
    }

    public void CalculateSlider()
    {
        _sliderSpawnCount.maxValue = (MapCreator.MapSixeX * MapCreator.MapSixeY) / 100;
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

    public void FullDiscript(bool isFool) => _fullDesctipt = isFool;
    private void printSproutDiscript(Text text, Sprout sprout)
    {
        text.text = "";
        if (_fullDesctipt)
        {
            text.text += sprout.Genome.GetDescription();
            text.text += "-----------\n";
        }
        else
        {
            text.text += "Performing operation: " + sprout.Genome.PerformingOperationNum + "\n";
            text.text += "Childs count: " + sprout.ChildsCount + "\n";
        }
        text.text += "Charge: " + sprout.Charge + "\n";
        text.text += "Charge change: " + sprout.ChargeChenge;
    }

    public void ShowSproutDescript(Sprout target)
    {
        _isSproutSelected = target;
        _targetSprout = target;
        _UiGenomTextFeeld.SetActive(_isSproutSelected);
    }

    public void SaveGenome()
    {
        int num = 1;
        string finalName = "";
        if (!_isSproutSelected || !_targetSprout) return;
        string savePath;
        savePath = Path.Combine(_baseSavePath, _saveName.text == "" ? "NewSprout" : _saveName.text);
        savePath = Path.ChangeExtension(savePath, _myExtension);
        while (File.Exists(savePath))
        {
            savePath = Path.Combine(_baseSavePath, _saveName.text == "" ? "NewSprout" : _saveName.text);
            savePath += num.ToString();
            savePath = Path.ChangeExtension(savePath, _myExtension);
            if (++num > 500) break;
        }
        finalName = Path.GetFileNameWithoutExtension(savePath);
        File.WriteAllText(savePath, _targetSprout.Genome.GetGenomeJson());
        UpdateLoadFiles();
        _loadFilesDropdown.value = _fileNames.FindIndex(val => val == finalName);
    }

    public void LoadGenome()
    {
        string loadPath = Path.Combine(_baseSavePath, _loadFilesDropdown.captionText.text);
        loadPath = Path.ChangeExtension(loadPath, _myExtension);
        if (_isSproutSelected && _targetSprout && File.Exists(loadPath)) _targetSprout.Genome.SetGenom(File.ReadAllText(loadPath));
    }

    public void UpdateLoadFiles()
    {
        string prevFileName = _loadFilesDropdown.captionText.text;
        _fileNames.Clear();
        foreach (var fileName in Directory.EnumerateFiles(_baseSavePath))
        {
            if (Path.GetExtension(fileName) == _myExtension) _fileNames.Add(Path.GetFileNameWithoutExtension(fileName));
        }
        _loadFilesDropdown.ClearOptions();
        _loadFilesDropdown.AddOptions(_fileNames);
        _loadFilesDropdown.value = _fileNames.FindIndex(val => val == prevFileName);
    }

    public void DeliteFile()
    {
        string delitePath = Path.Combine(_baseSavePath, _loadFilesDropdown.captionText.text);
        delitePath = Path.ChangeExtension(delitePath, _myExtension);
        if (File.Exists(delitePath)) File.Delete(delitePath);
        UpdateLoadFiles();
    }
}
