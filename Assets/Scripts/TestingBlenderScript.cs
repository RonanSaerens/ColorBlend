using Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

//this script just has an E keybind to test the blender color mixing
public class TestingBlenderScript : MonoBehaviour
{
    [SerializeField] private InputAction _inputAction;

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    private void Update()
    {
        if (_inputAction.triggered) 
        { 
            Singleton<Blender>.Instance.CalculateOutputColor();
        }
    }
}
