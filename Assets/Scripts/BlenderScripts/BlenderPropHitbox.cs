using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlenderPropHitbox : MonoBehaviour
{
    private Blender Blender { get; set; } = Singleton<Blender>.Instance;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    

    private void OnTriggerEnter (Collider other)
    {
        if (!other.gameObject.CompareTag("Prop")){ Debug.Log("is not prop tagged"); return; }

        Prop prop = other.gameObject.GetComponent<Prop>();

        if (!prop) { Debug.Log("Prop object not found"); return; }

        if (!prop.IsInBlender) 
        {
            Blender.AddPropToBlender(prop);
        }
        else { Debug.Log("Prop is already in blender."); }
    }
}
