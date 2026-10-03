using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Blender : MonoBehaviour
{
    public List<Prop> PropsInBlender { get; private set; } = new List<Prop>();

    private float _heatValue = 0.5f;
    public float HeatValue
    {
        get { return _heatValue; }
        private set
        {
            _heatValue = Mathf.Clamp01(_heatValue + _heatIncrement);
        }
    }

    private float _speedValue = 0.5f;
    public float SpeedValue { 
        get { return _speedValue; } 
        private set
        {
            _speedValue = Mathf.Clamp01(_speedValue + _speedIncrement);
        } 
    }

    private float _heatIncrement = 0.1f;
    private float _speedIncrement = 0.1f;

    public bool LidClosed { get; private set; }
    public bool IsOn { get; private set; }
    public bool TapOn { get; private set; }

    public void AddPropToBlender(Prop prop)
    {
        if (LidClosed) { Debug.Log("Lid is closed, didn't add prop"); return; }
        
        PropsInBlender.Add(prop);

        Debug.Log("Prop added to blender");
    }

    public void StartBlender()
    {
        
    }

    public void ToggleBlender()
    {
        IsOn = !IsOn;
    }

    public void ToggleTap()
    {
        TapOn = !TapOn;
    }

    public void IncrementSpeed(bool isFaster)
    {
        SpeedValue += isFaster ? _speedIncrement : -_speedIncrement;
    }
    public void IncrementHeat(bool isWarmer)
    {
        HeatValue += isWarmer ? _heatIncrement : -_heatIncrement;
    }
    public Color CalculateOutputColor()
    {
        Color averageColor = new Color();

        foreach (Prop prop in PropsInBlender)
        {

        }

        return averageColor;
    }
}
