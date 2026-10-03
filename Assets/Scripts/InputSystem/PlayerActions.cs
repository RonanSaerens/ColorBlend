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
    private Vector2 camMoveInput;

    private float _verticalRotation = 0f; //keeping track of vertical rotation

    [field: Space]
    [field: Header("Other")]
    [SerializeField] private Camera _firstPersonCamera;
    private float triggerDelayTimer = 0;

    [field: Space]
    [field: Header("Hands")]
    [SerializeField] private Transform heldItem;

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
        HandleCamMovement();
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

        camMoveInput = context.ReadValue<Vector2>();
    }
    
    private void HandleMovement()
    {
        Vector3 direction = moveInput.normalized;

        if (direction.magnitude > 0f)
        {
            controller.Move(transform.TransformDirection(direction) * Speed * Time.deltaTime);

        }
        if (heldItem != null)
        {
            heldItem.position = transform.GetChild(1).position;
            heldItem.rotation = Quaternion.identity;
            
        }
    }
    private void HandleCamMovement()
    {
        float horizontalRotation = camMoveInput.x * _rotationSpeed;
        float verticalRotation = camMoveInput.y * _rotationSpeed;

        //clamp vertical rotation to prevent flipping
        _verticalRotation = Mathf.Clamp(_verticalRotation - verticalRotation, _yLimit.x, _yLimit.y);

        //horizontal rotation (on player)
        transform.forward = Quaternion.Euler(0, horizontalRotation, 0) * transform.forward;

        //vertical rotation (on camera)
        _firstPersonCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (Time.time > triggerDelayTimer + 5)
        {
            if (heldItem != null)
            {
                heldItem.SetParent(null);
                var rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null) rb.useGravity = true;
                heldItem = null;
                return;
            }
            else
            {
                RaycastHit hit;
                Debug.Log("Interact button pressed");
                if (Physics.Raycast(_firstPersonCamera.transform.position, _firstPersonCamera.transform.forward, out hit))
                {
                    Debug.Log("Raycast hit: " + hit.transform.name);
                    if (hit.transform.CompareTag("Prop"))
                    {
                        Debug.Log("If statement successful");
                        PickUpFunction(hit.transform );
                    }
                }

            }
        }
       
        
    }

    public void PickUpFunction(Transform prop )
    {
        heldItem = prop;
        heldItem.SetParent(transform);
        heldItem.position = transform.GetChild(1).position; 

        var rb = prop.GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = false;

        
    }
}
