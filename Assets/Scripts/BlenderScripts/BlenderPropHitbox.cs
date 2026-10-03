using NUnit.Framework;
using Singleton;
using System.Collections.Generic;
using UnityEngine;

public class BlenderPropHitbox : MonoBehaviour
{
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
            Singleton<Blender>.Instance.AddPropToBlender(prop);
        }
        else { Debug.Log("Prop is already in blender."); }
    }
}
