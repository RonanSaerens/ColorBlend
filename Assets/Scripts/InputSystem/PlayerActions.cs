using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{

    [Header("Movement")]
    public float Speed = 10f;
    [SerializeField] private float inputDeadZone = 0.1f;
    private CharacterController controller;
    private Vector3 moveInput;

    [field: Space]
    [field: Header("Rotation")]
    [field: SerializeField] public bool RotationEnabled { get; set; } = true;
    [SerializeField, Range(0.01f, 1)] private float _rotationSpeed = 0.1f;
    [SerializeField] private Vector2 _yLimit = new Vector2(-40f, 80); //the top (but negative) & the bottom (but positive)

    private float _verticalRotation = 0f; //keeping track of vertical rotation

    [field: Space]
    [field: Header("Other")]
    [SerializeField] private Camera _firstPersonCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        Vector2 input = context.ReadValue<Vector2>();

        if (input.magnitude < inputDeadZone)
            input = Vector2.zero;


        //moveInput
        moveInput = new Vector3(input.x , 0f,input.y);
        
    }
    

    public void OnCameraMove(InputAction.CallbackContext context)
    {
        if (!RotationEnabled) { return; }

        Vector2 delta = context.ReadValue<Vector2>();

        float horizontalRotation = delta.x * _rotationSpeed;
        float verticalRotation = delta.y * _rotationSpeed;

        //clamp vertical rotation to prevent flipping
        _verticalRotation = Mathf.Clamp(_verticalRotation - verticalRotation, _yLimit.x, _yLimit.y);

        //horizontal rotation (on player)
        transform.forward = Quaternion.Euler(0, horizontalRotation, 0) * transform.forward;
             
        //vertical rotation (on camera)
        _firstPersonCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }
    
    private void HandleMovement()
    {
        Vector3 direction = moveInput.normalized;

        if (direction.magnitude > 0f)
        {
            controller.Move(transform.TransformDirection(direction) * Speed * Time.deltaTime);

        }
    }
}
