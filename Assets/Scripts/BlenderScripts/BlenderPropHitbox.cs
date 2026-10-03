using NUnit.Framework;
using Singleton;
using System.Collections.Generic;
using UnityEngine;

public class BlenderPropHitbox : MonoBehaviour
{
    [SerializeField] private Material _mat;
    void Start()
    {
        _mat.SetColor("_Verfkluer", Color.white);
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
            _mat.SetColor("_Verfkluer", Singleton<Blender>.Instance.CalculateOutputColor());

            prop.transform.gameObject.SetActive(false);
        }
        else { Debug.Log("Prop is already in blender."); }
    }
}
