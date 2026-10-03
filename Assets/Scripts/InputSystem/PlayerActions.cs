using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    [field: Header("UI")]
    [SerializeField] private UIDocument UIDoc;

    [field: Space]
    [field: Header("Hands")]
    public float ThrowForce = 0;
    [SerializeField] private Transform heldItem;
    private bool _throwItem = false;
    //private bool _interactTrigger = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleCamMovement();
        if (_throwItem)
        {
            ThrowForce += 0.2f;
            if (ThrowForce > 20)
            {
                ThrowForce = 20;
            }
        }
        else
        {
            ThrowForce = 0;
        }
        UIDoc.rootVisualElement.Q<Label>("ForceLabel").text = "Force: " + (int) ThrowForce;

        float ratio = ThrowForce / 20f;
        float percent = Mathf.Lerp(0, 100, ratio);
        UIDoc.rootVisualElement.Q<VisualElement>("ForceBar").style.width = Length.Percent(percent);
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
            heldItem.rotation = _firstPersonCamera.transform.rotation;


        }
        float verticalSpeed = Time.fixedDeltaTime * Physics.gravity.y;
        controller.Move(verticalSpeed * Time.fixedDeltaTime * Vector3.up);
        if (controller.isGrounded)
        {
            verticalSpeed = 0f;
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

    private void ThrowItem()
    {
        if (heldItem != null)
        {
            var rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.AddForce(_firstPersonCamera.transform.forward * ThrowForce, ForceMode.Impulse);
            }
            heldItem.SetParent(null);
            heldItem = null;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
        if (Time.time > triggerDelayTimer + 0.3f )
        {
            if (heldItem != null && _throwItem)
            {
                ThrowItem();
                _throwItem = false;
            }
            else if (heldItem != null && !_throwItem)
            {
                Debug.Log("Throw button pressed");
                _throwItem = true;
            }
            else
            {
                RaycastHit hit;
                Debug.Log("Interact button pressed");
                if (Physics.Raycast(_firstPersonCamera.transform.position, _firstPersonCamera.transform.forward, out hit))
                {
                    Debug.Log("Raycast hit: " + hit.transform.name);
                    Debug.Log("Raycast distance: " + hit.distance);
                    if (hit.transform.CompareTag("Prop") && hit.distance < 3)
                    {
                        Debug.Log("If statement successful");
                        PickUpFunction(hit.transform);
                    }
                }
            }
            triggerDelayTimer = Time.time;
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
