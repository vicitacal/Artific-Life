using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] UIController controllerUI;
    private Camera _camera;
    private Vector3 _mouseInput;
    private Transform _targetTransfotm;
    private Vector3 _targetPivitPoint;
    private Sprout _targetSprout;
    private Vector3 _offset;
    private float MaxX = MapCreator.MapSixeX;
    private float MaxZ = MapCreator.MapSixeY;
    private float MaxY = 30;
    private const float _defaultSpeed = 20;
    private const float _maxSpeed = 50;
    private const float _acceleration = 2;
    private bool _camMode = false;
    private const float _zoomMax = 8;
    private const float _zoomMin = 1;
    private float _sensitivity = 2;
    private float _limit = 80;
    private float _zoom = 0.25f;
    private float _orbitY = 0;
    private float _orbitX = 0;
    private bool _isMoving = false;
    private bool _isControl = false;
    private Vector3 _direction = new Vector3();
    public static CameraControl Instance;

    private void Awake()
    {
        Instance = this;
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cursor.visible = !Cursor.visible;
            _isControl = !_isControl;
        }
        if (Input.GetMouseButtonDown(1)) setCamMode(false);
        if (_isControl)
        {
            Cursor.lockState = CursorLockMode.None;
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
            Cursor.lockState = CursorLockMode.Locked;
            if (_camMode)
                orbitRotate();
            else
                normalRotate();
        }

        if (_camMode)
        {
            if (_targetTransfotm)
            {
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
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, -0.5f, MaxX), Mathf.Clamp(newPosition.y, 0.37f, MaxY), Mathf.Clamp(newPosition.z, -0.5f, MaxZ));
    }
    
    private void normalMove()
    {
        bool isPrest = false;
        if (Input.GetKey(KeyCode.W))
        {
            isPrest = true;
            _isMoving = true;
            _direction += transform.forward * _acceleration * Time.deltaTime * ((Input.GetKey(KeyCode.LeftShift) ? _maxSpeed : _defaultSpeed) - _direction.magnitude);
            _direction += (transform.forward - _direction.normalized) * Time.deltaTime * 20;
        }
        if (Input.GetKey(KeyCode.S))
        {
            isPrest = true;
            _isMoving = true;
            _direction -= transform.forward * _acceleration * Time.deltaTime * ((Input.GetKey(KeyCode.LeftShift) ? _maxSpeed : _defaultSpeed) - _direction.magnitude);
            _direction += (-transform.forward - _direction.normalized) * Time.deltaTime * 20;
        }
        if (Input.GetKey(KeyCode.A)) 
        {
            isPrest = true;
            _isMoving = true;
            _direction -= transform.right * _acceleration * Time.deltaTime * ((Input.GetKey(KeyCode.LeftShift) ? _maxSpeed : _defaultSpeed) - _direction.magnitude);
            _direction += (-transform.right - _direction.normalized) * Time.deltaTime * 20;
        }
        if (Input.GetKey(KeyCode.D))
        {
            isPrest = true;
            _isMoving = true;
            _direction += transform.right * _acceleration * Time.deltaTime * ((Input.GetKey(KeyCode.LeftShift) ? _maxSpeed : _defaultSpeed) - _direction.magnitude);
            _direction += (transform.right - _direction.normalized) * Time.deltaTime * 20;
        }
        if (isPrest)
        {
            applyPosition(transform.position + _direction * Time.deltaTime);
        }
        else
        {
            if (_isMoving)
            {
                _direction -= _direction * Time.deltaTime * _acceleration;
                applyPosition(transform.position + _direction * Time.deltaTime);
                if (_direction.magnitude < 0.1) _isMoving = false;
            }
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
            controllerUI.ShowSproutDescript(_targetSprout);
            controllerUI.UpdateLoadFiles();
        }
        else
        {
            _mouseInput = transform.rotation.eulerAngles;
            controllerUI.ShowSproutDescript(null);
        }
        _camMode = inVal;
    }

    public void UpdateMapSize()
    {
        MaxX = MapCreator.MapSixeX - 0.5f;
        MaxZ = MapCreator.MapSixeY - 0.5f;
        MaxY = Mathf.Sqrt(MapCreator.MapSixeY * MapCreator.MapSixeX) * 0.5f;
        applyPosition(transform.position);
    }
}
