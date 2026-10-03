using UnityEngine;

public class Prop : MonoBehaviour
{
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public int PropSize { get; private set; } = 1;
    [field: SerializeField] public bool IsInBlender { get; set; }
    [field: SerializeField] public Material Material { get; private set; }

    
    void Start()
    {
        if (!Material) { return; }
        Material.color = Color;
    }
}
