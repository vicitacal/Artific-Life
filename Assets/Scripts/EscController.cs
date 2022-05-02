using UnityEngine;
using UnityEngine.UI;
using System;

public class EscController : MonoBehaviour
{
    [SerializeField] private InputField _inputX;
    [SerializeField] private InputField _inputY;
    [SerializeField] private InputField _chargeStrive;
    [SerializeField] private InputField _equalizationStep;
    [SerializeField] private InputField _chargeSpendSprout;
    [SerializeField] private InputField _chargeSpendSleep;
    [SerializeField] private InputField _maxLight;
    [SerializeField] private InputField _organicVolume;
    [SerializeField] private InputField _chargeVolume;
    [SerializeField] private InputField _genomeLenght;
    [SerializeField] private InputField _childCostMin;
    [SerializeField] private InputField _childCostMax;
    [SerializeField] private InputField _sproutCostMin;
    [SerializeField] private InputField _sproutCostMax;
    [SerializeField] private InputField _conditionArgument;
    [SerializeField] private InputField _chargeSpendLeaf;
    [SerializeField] private InputField _chargeSpendRoot;
    [SerializeField] private InputField _chargeSpendAntenna;
    [SerializeField] private InputField _organicTreshold;
    [SerializeField] private InputField _energyTreshold;
    [SerializeField] private InputField _mutationChance;
    [SerializeField] private GameObject _buttonPanel;
    [SerializeField] private GameObject _settingsPanel;
    public static EscController Instance;
    private Map _map;

    private void Awake()
    {
        Instance = this;
        _map = FindObjectOfType<Map>();
    }

    private void Start()
    {
        MapCreator.MapSixeX = PlayerPrefs.GetInt("inputX", 100);
        MapCreator.MapSixeY = PlayerPrefs.GetInt("inputY", 100);
        Charge.EqualizationTo = PlayerPrefs.GetInt("chargeStrive", 100);
        Charge.EqualizationStep = PlayerPrefs.GetFloat("equalizationStep", 1f);
        Sprout.DefaultChargeSpend = PlayerPrefs.GetInt("chargeSpendSprout", 20);
        Sprout.SleepChargeSpend = PlayerPrefs.GetInt("chargeSpendSleep", 2);
        Illumination.Multiplayer = PlayerPrefs.GetInt("maxLight", 65);
        Creature.OrganicVolume = PlayerPrefs.GetInt("organicVolume", 10);
        Creature.ChargeVolume = PlayerPrefs.GetInt("chargeVolume", 15);
        Genome.GenomeLenght = PlayerPrefs.GetInt("genomeLenght", 24);
        Genome.Comand.MaxChildCost = PlayerPrefs.GetInt("childCostMax", 200);
        Genome.Comand.MinChildCost = PlayerPrefs.GetInt("childCostMin", 50); 
        Genome.Comand.MaxSproutCost = PlayerPrefs.GetInt("sproutCostMax", 450);
        Genome.Comand.MinSproutCost = PlayerPrefs.GetInt("sproutCostMin", 200); 
        Genome.Comand.MaxConditionArgument = PlayerPrefs.GetInt("conditionArgument", 500);
        Leaf.LeafEnergySpend = PlayerPrefs.GetInt("chargeSpendLeaf", 13);
        Root.RootEnergySpend = PlayerPrefs.GetInt("chargeSpendRoot", 10);
        Antenna.AntennaEnergySpend = PlayerPrefs.GetInt("chargeSpendAntenna", 12);
        Creature.OrganicDamageTreshold = PlayerPrefs.GetInt("organicTreshold", 300);
        Creature.EnergyDamageTreshold = PlayerPrefs.GetInt("energyTreshold", 200);
        Sprout.MutateChanse = PlayerPrefs.GetFloat("mutationChance", 0.8f);
        ReinitAll();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) CloseEsc();
    }

    public void ReinitAll()
    {
        UIController.Instance.ClearScene();
        MapCreator.Instance.InitMap();
        CameraControl.Instance.UpdateMapSize();
        _map.InitArrays();
        _map.InitMap();
        _map.InitTexture();
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void CloseEsc()
    {
        _settingsPanel.SetActive(false);
        _buttonPanel.SetActive(true);
        UIController.Instance.Pause(false);
        CameraControl.Instance.enabled = true;
        UIController.Instance.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        UIController.Instance.ClearScene();
        MapCreator.Tick.RemoveAllListeners();
        CloseEsc();
    }

    public void ApplySettings()
    {
        string equalizationStep = _equalizationStep.text;
        if (equalizationStep.Contains(".")) equalizationStep = equalizationStep.Replace(".", ",");
        string mutationChanse = _mutationChance.text;
        if (mutationChanse.Contains(".")) mutationChanse = mutationChanse.Replace(".", ",");
        MapCreator.MapSixeX = Mathf.Clamp(System.Convert.ToInt32(_inputX.text == "" ? "0" : _inputX.text), 4, 2000);
        MapCreator.MapSixeY = Mathf.Clamp(System.Convert.ToInt32(_inputY.text == "" ? "0" : _inputY.text), 4, 1500);
        Charge.EqualizationTo = Mathf.Clamp(System.Convert.ToInt32(_chargeStrive.text == "" ? "0" : _chargeStrive.text), 0, 10000);
        Charge.EqualizationStep = Mathf.Clamp(System.Convert.ToSingle(equalizationStep == "" ? "0" : equalizationStep), 0.001f, 100f);
        Sprout.DefaultChargeSpend = Mathf.Clamp(System.Convert.ToInt32(_chargeSpendSprout.text == "" ? "0" : _chargeSpendSprout.text), 1, 1000);
        Sprout.SleepChargeSpend = Mathf.Clamp(System.Convert.ToInt32(_chargeSpendSleep.text == "" ? "0" : _chargeSpendSleep.text), 0, Sprout.DefaultChargeSpend);
        Illumination.Multiplayer = Mathf.Clamp(System.Convert.ToInt32(_maxLight.text == "" ? "0" : _maxLight.text), 0, 10000);
        Creature.OrganicVolume = Mathf.Clamp(System.Convert.ToInt32(_organicVolume.text == "" ? "0" : _organicVolume.text), 0, 10000);
        Creature.ChargeVolume = Mathf.Clamp(System.Convert.ToInt32(_chargeVolume.text == "" ? "0" : _chargeVolume.text), 0, 10000);
        Genome.GenomeLenght = Mathf.Clamp(System.Convert.ToInt32(_genomeLenght.text == "" ? "0" : _genomeLenght.text), 2, 50);
        Genome.Comand.MaxChildCost = Mathf.Clamp(System.Convert.ToInt32(_childCostMax.text == "" ? "0" : _childCostMax.text), 1, 10000);
        Genome.Comand.MinChildCost = Mathf.Clamp(System.Convert.ToInt32(_childCostMin.text == "" ? "0" : _childCostMin.text), 1, Genome.Comand.MaxChildCost);
        Genome.Comand.MaxSproutCost = Mathf.Clamp(System.Convert.ToInt32(_sproutCostMax.text == "" ? "0" : _sproutCostMax.text), 1, 10000);
        Genome.Comand.MinSproutCost = Mathf.Clamp(System.Convert.ToInt32(_sproutCostMin.text == "" ? "0" : _sproutCostMin.text), 1, Genome.Comand.MaxSproutCost);
        Genome.Comand.MaxConditionArgument = Mathf.Clamp(System.Convert.ToInt32(_conditionArgument.text == "" ? "0" : _conditionArgument.text), 1, 10000);
        Leaf.LeafEnergySpend = Mathf.Clamp(System.Convert.ToInt32(_chargeSpendLeaf.text == "" ? "0" : _chargeSpendLeaf.text), 1, 10000);
        Root.RootEnergySpend = Mathf.Clamp(System.Convert.ToInt32(_chargeSpendRoot.text == "" ? "0" : _chargeSpendRoot.text), 1, 10000);
        Antenna.AntennaEnergySpend = Mathf.Clamp(System.Convert.ToInt32(_chargeSpendAntenna.text == "" ? "0" : _chargeSpendAntenna.text), 1, 10000);
        Creature.OrganicDamageTreshold = Mathf.Clamp(System.Convert.ToInt32(_organicTreshold.text == "" ? "0" : _organicTreshold.text), 1, 10000);
        Creature.EnergyDamageTreshold = Mathf.Clamp(System.Convert.ToInt32(_energyTreshold.text == "" ? "0" : _energyTreshold.text), 1, 10000);
        Sprout.MutateChanse = Mathf.Clamp(System.Convert.ToSingle(mutationChanse == "" ? "0" : mutationChanse), 0, 1f);
        SaveSettings();
        ShowSettings();
        ReinitAll();
    }
    public void ShowSettings()
    {
        _inputX.text = MapCreator.MapSixeX.ToString();
        _inputY.text = MapCreator.MapSixeY.ToString();
        _chargeStrive.text = Charge.EqualizationTo.ToString();
        _equalizationStep.text = Charge.EqualizationStep.ToString("0.0");
        _chargeSpendSprout.text = Sprout.DefaultChargeSpend.ToString();
        _chargeSpendSleep.text = Sprout.SleepChargeSpend.ToString();
        _maxLight.text = Illumination.Multiplayer.ToString();
        _organicVolume.text = Creature.OrganicVolume.ToString();
        _chargeVolume.text = Creature.ChargeVolume.ToString();
        _genomeLenght.text = Genome.GenomeLenght.ToString();
        _childCostMin.text = Genome.Comand.MinChildCost.ToString();
        _childCostMax.text = Genome.Comand.MaxChildCost.ToString();
        _sproutCostMin.text = Genome.Comand.MinSproutCost.ToString();
        _sproutCostMax.text = Genome.Comand.MaxSproutCost.ToString();
        _conditionArgument.text = Genome.Comand.MaxConditionArgument.ToString();
        _chargeSpendLeaf.text = Leaf.LeafEnergySpend.ToString();
        _chargeSpendRoot.text = Root.RootEnergySpend.ToString();
        _chargeSpendAntenna.text = Antenna.AntennaEnergySpend.ToString();
        _organicTreshold.text = Creature.OrganicDamageTreshold.ToString();
        _energyTreshold.text = Creature.EnergyDamageTreshold.ToString();
        _mutationChance.text = Sprout.MutateChanse.ToString("0.0");
    }

    public void RestorDefault()
    {
        _inputX.text = "100";
        _inputY.text = "100";
        _chargeStrive.text = "100";
        _equalizationStep.text = "1,0";
        _chargeSpendSprout.text = "20";
        _chargeSpendSleep.text = "2";
        _maxLight.text = "65";
        _organicVolume.text = "10";
        _chargeVolume.text = "15";
        _genomeLenght.text = "24";
        _childCostMin.text = "50";
        _childCostMax.text = "200";
        _sproutCostMin.text = "200";
        _sproutCostMax.text = "450";
        _conditionArgument.text = "500";
        _chargeSpendLeaf.text = "13";
        _chargeSpendRoot.text = "10";
        _chargeSpendAntenna.text = "12";
        _organicTreshold.text = "300";
        _energyTreshold.text = "200";
        _mutationChance.text = "0,8";
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("inputX", MapCreator.MapSixeX);
        PlayerPrefs.SetInt("inputY", MapCreator.MapSixeY);
        PlayerPrefs.SetInt("chargeStrive", Charge.EqualizationTo);
        PlayerPrefs.SetFloat("equalizationStep", Charge.EqualizationStep);
        PlayerPrefs.SetInt("chargeSpendSprout", Sprout.DefaultChargeSpend);
        PlayerPrefs.SetInt("chargeSpendSleep", Sprout.SleepChargeSpend);
        PlayerPrefs.SetInt("maxLight", Illumination.Multiplayer);
        PlayerPrefs.SetInt("organicVolume", Creature.OrganicVolume);
        PlayerPrefs.SetInt("chargeVolume", Creature.ChargeVolume);
        PlayerPrefs.SetInt("genomeLenght", Genome.GenomeLenght);
        PlayerPrefs.SetInt("childCostMin", Genome.Comand.MinChildCost);
        PlayerPrefs.SetInt("childCostMax", Genome.Comand.MaxChildCost);
        PlayerPrefs.SetInt("sproutCostMin", Genome.Comand.MinSproutCost);
        PlayerPrefs.SetInt("sproutCostMax", Genome.Comand.MaxSproutCost);
        PlayerPrefs.SetInt("conditionArgument", Genome.Comand.MaxConditionArgument);
        PlayerPrefs.SetInt("chargeSpendLeaf", Leaf.LeafEnergySpend);
        PlayerPrefs.SetInt("chargeSpendRoot", Root.RootEnergySpend);
        PlayerPrefs.SetInt("chargeSpendAntenna", Antenna.AntennaEnergySpend);
        PlayerPrefs.SetInt("organicTreshold", Creature.OrganicDamageTreshold);
        PlayerPrefs.SetInt("energyTreshold", Creature.EnergyDamageTreshold);
        PlayerPrefs.SetFloat("mutationChance", Sprout.MutateChanse);
        PlayerPrefs.Save();
    }

}
