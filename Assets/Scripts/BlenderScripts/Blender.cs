using Singleton;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Blender
{
    public List<Prop> PropsInBlender { get; private set; } = new List<Prop>();

    private float _heatValue = 1f;
    public float HeatValue
    {
        get { return _heatValue; }
        private set
        {
            _heatValue = Mathf.Clamp(value, 0, 2);
        }
    }

    private float _speedValue = 1f;
    public float SpeedValue { 
        get { return _speedValue; } 
        private set
        {
            _speedValue = Mathf.Clamp(value, 0, 2);
        } 
    }

    private float _heatIncrement = 0.1f;
    private float _speedIncrement = 0.1f;

    public bool LidClosed { get; private set; }
    public bool IsOn { get; private set; }
    public bool TapOn { get; private set; }

    [SerializeField] private BlenderPropHitbox BlenderPropHitbox;

    public void AddPropToBlender(Prop prop)
    {
        if (LidClosed) { Debug.Log("Lid is closed, didn't add prop"); return; }
        
        PropsInBlender.Add(prop);

        Debug.Log("Prop added to blender");
    }

    public void StartBlender()
    {
        IsOn = true;
    }

    public void ToggleBlender(bool isOn)
    {
        IsOn = isOn;
    }

    public void ToggleTap()
    {
        TapOn = !TapOn;
    }

    public void IncrementSpeed(bool isFaster)
    {
        if (isFaster)
        {
            SpeedValue += _speedIncrement;
        }
        else
        {
            SpeedValue -= _speedIncrement;
        }
    }
    public void IncrementHeat(bool isWarmer)
    {
        if (isWarmer)
        {
            HeatValue += _heatIncrement;
        }
        else
        {
            HeatValue -= _heatIncrement;
        }
    }
    public Color CalculateOutputColor()
    {
        if (PropsInBlender.Count == 0) { return Color.white; }

        List<Color> AllPropColors = new List<Color>();
        
        //adding all colors to a color list, depending no the size I add the color of a prop multiple times
        foreach (Prop prop in PropsInBlender)
        {
            for (int i = 0; i < prop.PropSize; i++) 
            {
                AllPropColors.Add(prop.Color);
            }
        }

        float r = 0;
        float g = 0;
        float b = 0;

        //adding all r, g, and b values of all colors together, then averaging them out by the amount of total colors
        foreach (Color color in AllPropColors)
        {
            r += Mathf.Pow(color.r, 2);
            g += Mathf.Pow(color.g, 2);
            b += Mathf.Pow(color.b, 2);
        }

        Color averagedColor = new Color(Mathf.Sqrt(r / AllPropColors.Count), Mathf.Sqrt(g / AllPropColors.Count), Mathf.Sqrt(b / AllPropColors.Count));

        //adding 'speed' adjustment (edits saturation)
        Color grayscale = new Color(averagedColor.grayscale, averagedColor.grayscale, averagedColor.grayscale);
        Color saturatedColor = Color.LerpUnclamped(grayscale, averagedColor, 1.6f); //160% saturated
        Debug.Log(
            $"AveragedColor: {averagedColor}\n" +
            $"Grayscale: {grayscale}\n" +
            $"SaturatedColor: {saturatedColor}"
            );

        return averagedColor;
    }
}
