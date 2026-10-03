using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{

    [Header("Movement")]
    public float Speed = 10f;
    [SerializeField] private float inputDeadZone = 0.1f;
    private CharacterController controller;
    private Vector3 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
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


        moveInput = new Vector3(input.x, 0f, input.y);
    }
    
    private void HandleMovement()
    {
        Vector3 direction = moveInput.normalized;

        if (direction.magnitude > 0f)
        {
            controller.Move(direction * Speed * Time.deltaTime);

        }
    }
}
