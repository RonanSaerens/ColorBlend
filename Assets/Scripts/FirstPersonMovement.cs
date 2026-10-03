using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [field: Header("Movement")]
    [field: SerializeField] public bool MovementEnabled { get; set; } = true;

    [SerializeField] private float _movementSpeed = 5;
    private InputAction _movement = new InputAction();

    [field: Space]
    [field: Header("Rotation")]
    [field: SerializeField] public bool RotationEnabled { get; set; } = true;
    [SerializeField, Range(0.01f, 1)] private float _rotationSpeed = 0.1f;
    [SerializeField] private Vector2 _yLimit = new Vector2(-40f, 80); //the top (but negative) & the bottom (but positive)
    
    private InputAction _mouseDelta = new InputAction(type: InputActionType.Value, binding: "<Pointer>/delta");

    private float _verticalRotation = 0f; //keeping track of vertical rotation

    [field: Space]
    [field: Header("Jumping")]
    [SerializeField] private float _jumpPower = 2;
    private float _verticalVelocity = 0f;
    private float _gravity = -9.81f;
    private float _groundedMarginTime = 0.2f;
    private float _lastTimeGrounded;

    private InputAction _jump = new InputAction();

    [Space]
    [Header("Other")]
    [SerializeField] private Camera _firstPersonCamera;
    private CharacterController _characterController;

    private Vector3 _totalDirection;    //the direction the user moves to in that frame

    private void OnEnable()
    {
        _mouseDelta.Enable();
        _movement.Enable();
        _jump.Enable();
    }

    private void OnDisable()
    {
        _mouseDelta.Disable();
        _movement.Disable();
        _jump.Disable();
    }

    private void Awake()
    {
        _firstPersonCamera = _firstPersonCamera is null ? Camera.main : _firstPersonCamera;

        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        _movement.AddCompositeBinding("3DVector(mode=2)")
        .With("Forward", "<Keyboard>/w")
        .With("Left", "<Keyboard>/a")
        .With("Backward", "<Keyboard>/s")
        .With("Right", "<Keyboard>/d");

        _jump.AddBinding("<Keyboard>/space");
    }

    void Update()
    {
        Vector3 movement = Movement();

        _totalDirection += movement;

        if (_characterController.isGrounded)
        {
            _lastTimeGrounded = Time.time;
        }

        Jump();
        _totalDirection.y = _verticalVelocity;
        
        Rotation();

        _characterController.Move(_totalDirection * Time.deltaTime);

        _totalDirection = Vector3.zero; //resetting direction
    }

    private Vector3 Movement()
    {
        if (!MovementEnabled) { return Vector3.zero; }

        Vector3 direction = _movement.ReadValue<Vector3>().normalized;
        direction = transform.TransformDirection(direction) * _movementSpeed;

        return direction;
    }

    private void Rotation()
    {
        if (!RotationEnabled) { return; }

        Vector2 delta = _mouseDelta.ReadValue<Vector2>();

        float horizontalRotation = delta.x * _rotationSpeed;
        float verticalRotation = delta.y * _rotationSpeed;

        //clamp vertical rotation to prevent flipping
        _verticalRotation = Mathf.Clamp(_verticalRotation - verticalRotation, _yLimit.x, _yLimit.y);

        //horizontal rotation (on player)
        transform.Rotate(Vector3.up * horizontalRotation, Space.World);

        //vertical rotation (on camera)
        _firstPersonCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    private void Jump()
    {
        bool isActuallyGrounded = Time.time - _lastTimeGrounded < _groundedMarginTime;

        if (isActuallyGrounded)
        {
            _verticalVelocity = 0f;

            if (_jump.WasPressedThisFrame())
            {
                _verticalVelocity = _jumpPower;
                _lastTimeGrounded = -999f; //no double jumps
            }
        }

        _verticalVelocity += _gravity * Time.deltaTime;
    }
}
