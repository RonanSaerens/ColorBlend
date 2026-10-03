using Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

//this script just has an E keybind to test the blender color mixing
public class BlenderObject : MonoBehaviour
{
    private Blender _blender;

    [SerializeField] private GameObject _colorExtract;
    [SerializeField] private GameObject _rotor;
    [SerializeField] private GameObject _lidAnchor;
    [SerializeField] private GameObject _tap;

    private int _rotorSpeed = 900;
    private float _lidSpeed = 180f;
    private float _tapSpeed = 180f;

    private void Awake()
    {
        _blender = Singleton<Blender>.Instance;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (_blender.IsOn)
        {
            _blender.ToggleBlender(true);

            _rotor.transform.RotateAround(_rotor.transform.position, Vector3.up, Time.deltaTime * _rotorSpeed);

            Quaternion targetLidRotation = Quaternion.Euler(0f, 0f, 0f);
            _lidAnchor.transform.localRotation = Quaternion.RotateTowards(_lidAnchor.transform.localRotation, targetLidRotation, Time.deltaTime * _lidSpeed);
        }
        else 
        {
            _blender.ToggleBlender(false);

            Quaternion targetLidRotation = Quaternion.Euler(0f, -150f, 0f);
            _lidAnchor.transform.localRotation = Quaternion.RotateTowards(_lidAnchor.transform.localRotation, targetLidRotation, Time.deltaTime * _lidSpeed);
        }

        if (_blender.TapOn)
        {
            Quaternion targetTapRotation = Quaternion.Euler(0f, 90f, 0f);
            _tap.transform.localRotation = Quaternion.RotateTowards(_tap.transform.localRotation, targetTapRotation, Time.deltaTime * _tapSpeed);
        }
        else
        {
            Quaternion targetTapRotation = Quaternion.Euler(0f, 0f, 0f);
            _tap.transform.localRotation = Quaternion.RotateTowards(_tap.transform.localRotation, targetTapRotation, Time.deltaTime * _tapSpeed);
        }

        if (_blender.TapOn)
        {
            Color outputColor = Singleton<Blender>.Instance.CalculateOutputColor();
            _colorExtract.GetComponent<MeshRenderer>().material.color = outputColor;
        }
    }
}
