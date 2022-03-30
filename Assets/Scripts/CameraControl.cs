using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Text _statusText;
    [SerializeField] private GameObject _UiGenomTextFeeld;
    private Camera _camera;
    private Vector3 _mouseInput;
    private Transform _targetTransfotm;
    private Vector3 _targetPivitPoint;
    private Sprout _targetSprout;
    private Vector3 _offset;
    private readonly float MaxX = MapCreator.MapSixeX * MapCreator.StepLenght;
    private readonly float MaxZ = MapCreator.MapSixeY * MapCreator.StepLenght;
    private readonly float MaxY = 10;
    private bool _camMode = false;
    private const float _zoomMax = 8;
    private const float _zoomMin = 1;
    private float _sensitivity = 2;
    private float _limit = 80;
    private float _zoom = 0.25f;
    private float _orbitY = 0;
    private float _orbitX = 0;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        Cursor.visible = false;
    }

    private void Start()
    {
        _offset = new Vector3(_offset.x, _offset.y, -Mathf.Abs(_zoomMax) / 2);
        _mouseInput = transform.rotation.eulerAngles;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) Cursor.visible = true;
        if (Input.GetKeyUp(KeyCode.LeftControl)) Cursor.visible = false;
        if (Input.GetMouseButtonDown(1)) setCamMode(false);
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (tryGetSproutTransform(ref _targetTransfotm))
                {
                    setCamMode(true);
                    _targetPivitPoint = transform.localRotation * -_offset + transform.position;
                }
            }
        }
        else
        {
            if (_camMode)
                orbitRotate();
            else
                normalRotate();
        }

        if (_camMode)
        {
            if (_targetTransfotm)
            {
                printSproutDiscript();
                orbitMove(_targetPivitPoint);
                _targetPivitPoint += (_targetTransfotm.position - _targetPivitPoint) * Time.deltaTime * 5f;
            }
            else
                setCamMode(false);
        }
        else
        {
            normalMove();
        }
    }

    private void applyPosition(Vector3 newPosition)
    {
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, 0, MaxX), Mathf.Clamp(newPosition.y, 0.36f, MaxY), Mathf.Clamp(newPosition.z, 0, MaxZ));
    }
    
    private void normalMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            applyPosition(transform.position + transform.forward * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 5f : 2f));
        }
        if (Input.GetKey(KeyCode.S))
        {
            applyPosition(transform.position + transform.forward * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? -5f : -2f));
        }
        if (Input.GetKey(KeyCode.A))
        {
            applyPosition(transform.position + transform.right * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? -5f : -2f));
        }
        if (Input.GetKey(KeyCode.D))
        {
            applyPosition(transform.position + transform.right * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 5f : 2f));
        }
    }
    
    private void normalRotate()
    {
        _mouseInput += new Vector3(Input.GetAxis("Mouse Y") * -_sensitivity, Input.GetAxis("Mouse X") * _sensitivity, 0f);
        transform.rotation = Quaternion.Euler(_mouseInput);
    }
    
    private void orbitRotate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) _offset.z += _zoom;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) _offset.z -= _zoom;
        _offset.z = Mathf.Clamp(_offset.z, -Mathf.Abs(_zoomMax), -Mathf.Abs(_zoomMin));

        _orbitY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitivity;
        _orbitX += Input.GetAxis("Mouse Y") * _sensitivity;
        _orbitX = Mathf.Clamp(_orbitX, -_limit, _limit);
        transform.localEulerAngles = new Vector3(-_orbitX, _orbitY, 0);
    }
    
    private void orbitMove(Vector3 pivitPoint)
    {
        applyPosition(transform.localRotation * _offset + pivitPoint);
    }

    private bool tryGetSproutTransform(ref Transform transform)
    {
        RaycastHit clickInfo;
        Sprout targetSprout;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out clickInfo))
        {
            if (clickInfo.collider.TryGetComponent<Sprout>(out targetSprout))
            {
                _targetSprout = targetSprout;
                transform = targetSprout.transform;
                return true;
            }
        }
        return false;
    }

    private void setCamMode(bool inVal)
    {
        if (inVal)
        {
            _orbitX = -transform.localEulerAngles.x;
            _UiGenomTextFeeld.SetActive(true);
        }
        else
        {
            _mouseInput = transform.rotation.eulerAngles;
            _statusText.text = "";
            _UiGenomTextFeeld.SetActive(false);
        }
        _camMode = inVal;
    }

    private void printSproutDiscript()
    {
        _statusText.text = "";
        _statusText.text += _targetSprout.Genome.GetDescription();
        _statusText.text += "-----------\n";
        _statusText.text += "Charge: " + _targetSprout.Charge + "\n";
        _statusText.text += "Charge change: " + _targetSprout.ChargeChenge;

    }
}
