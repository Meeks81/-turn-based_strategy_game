using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _rotateSensetive;
    [SerializeField] private float _zoomSensetive;
    [Space]
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _zoom;

    public float zoom
    {
        get => _zoom;
        set
        {
            _zoom = Mathf.Clamp(value, _minZoom, _maxZoom);
            UpdateHeight();
        }
    }

    private float _xAngle;
    private float _yAngle;

    private void Start()
    {
        Rotate(Vector2.zero);
        zoom += 0;
        UpdateHeight();
    }

    private void Update()
    {
        Vector2 movement = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            movement.y++;
        if (Input.GetKey(KeyCode.S))
            movement.y--;
        if (Input.GetKey(KeyCode.A))
            movement.x--;
        if (Input.GetKey(KeyCode.D))
            movement.x++;

        float speed = Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : _speed;

        Move(movement * speed);

        if (Input.GetMouseButton(1))
        {
            float xRotate = Input.GetAxis("Mouse Y");
            float yRotate = Input.GetAxis("Mouse X");
            Rotate(new Vector2(-xRotate, yRotate) * _rotateSensetive);
        }

        zoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSensetive;
    }

    public void Rotate(Vector2 rotation)
    {
        _xAngle = Mathf.Clamp(_xAngle + rotation.x, -89f, 89f);
        _yAngle += rotation.y;
        transform.eulerAngles = new Vector3(_xAngle, _yAngle);
    }

    public void Move(Vector2 movement)
    {
        Vector3 direction = transform.TransformDirection(movement);
        direction.y = 0;
        direction = direction.normalized;
        transform.position += direction * movement.magnitude * (1f + zoom / 2f) * Time.deltaTime;

        UpdateHeight();
    }

    private void UpdateHeight()
    {
        Vector3 position = transform.position;
        position.y = zoom;
        transform.position = position;
    }

}
