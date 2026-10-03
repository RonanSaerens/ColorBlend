using UnityEngine;

public class Prop : MonoBehaviour
{
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public int PropSize { get; private set; } = 1;
    [field: SerializeField] public bool IsInBlender { get; set; }

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
