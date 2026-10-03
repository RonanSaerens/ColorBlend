using Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

//this script just has an E keybind to test the blender color mixing
public class BlenderObject : MonoBehaviour
{
    private Blender _blender;

    [SerializeField] private InputAction _inputAction;

    [SerializeField] private GameObject _colorExtract;
    [SerializeField] private GameObject _rotor;
    [SerializeField] private GameObject _lidAnchor;

    private int _rotorSpeed = 900;
    private float _lidSpeed = 180f;

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    private void Awake()
    {
        _blender = Singleton<Blender>.Instance;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (_inputAction.triggered) 
        {
            _blender.ToggleBlender();
            _colorExtract.GetComponent<MeshRenderer>().material.color = Singleton<Blender>.Instance.CalculateOutputColor();
        }

        if (_blender.IsOn)
        {
            _rotor.transform.RotateAround(_rotor.transform.position, Vector3.up, Time.deltaTime * _rotorSpeed);

            Quaternion targetLidRotation = Quaternion.Euler(0f, 0f, 0f);
            _lidAnchor.transform.localRotation = Quaternion.RotateTowards(_lidAnchor.transform.localRotation, targetLidRotation, Time.deltaTime * _lidSpeed);
        }
        else 
        {
            Quaternion targetLidRotation = Quaternion.Euler(0f, -150f, 0f);
            _lidAnchor.transform.localRotation = Quaternion.RotateTowards(_lidAnchor.transform.localRotation, targetLidRotation, Time.deltaTime * _lidSpeed);
        }
    }
}
