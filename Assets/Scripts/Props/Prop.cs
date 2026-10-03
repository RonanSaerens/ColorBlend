using System.Linq;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public int PropSize { get; private set; } = 1;
    [field: SerializeField] public bool IsInBlender { get; set; }
    [field: SerializeField] public Material Material { get; private set; }
    private Renderer rend;


    void Awake()
    {
        rend = GetComponent<Renderer>();
        Material mat = new Material(Material.shader);
        mat.color = Color;
        Material[] materials = rend.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].color == Material.color)
            {
                //var block = new MaterialPropertyBlock();
                materials[i] = mat;
                rend.materials = materials;
                //block.SetColor("_BaseColor", Color);
                //rend.SetPropertyBlock(block, i);

                break;
            }   
        }
    }   
}
     