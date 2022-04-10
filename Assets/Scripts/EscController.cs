using UnityEngine;
using UnityEngine.UI;

public class EscController : MonoBehaviour
{
    [SerializeField] private InputField _inputX;
    [SerializeField] private InputField _inputY;
    public static EscController Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIController.Instance.Pause();
            CameraControl.Instance.enabled = true;
            UIController.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void ChengeMapSize()
    {
        Map map = FindObjectOfType<Map>();
        MapCreator.MapSixeX = Mathf.Clamp(System.Convert.ToInt32(_inputX.text), 4, 2000);
        MapCreator.MapSixeY = Mathf.Clamp(System.Convert.ToInt32(_inputY.text), 4, 1500);
        MapCreator.Instance.InitMap();
        CameraControl.Instance.UpdateMapSize();
        map.InitArrays();
        map.ClearMap();
        map.InitMap();
        map.InitTexture();
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
