using UnityEngine;

public class Prop : MonoBehaviour
{
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public PropSize PropSize { get; private set; }
    [field: SerializeField] public bool IsInBlender { get; set; }

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
