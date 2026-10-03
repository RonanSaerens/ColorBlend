using Singleton;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Blender
{
    public List<Prop> PropsInBlender { get; private set; } = new List<Prop>();

    private float _heatValue = 1;
    public float HeatValue
    {
        get { return _heatValue; }
        private set
        {
            _heatValue = Mathf.Clamp01(_heatValue + _heatIncrement);
        }
    }

    private float _speedValue = 1;
    public float SpeedValue { 
        get { return _speedValue; } 
        private set
        {
            _speedValue = Mathf.Clamp01(_speedValue + _speedIncrement);
        } 
    }

    private float _heatIncrement = 0.1f;
    private float _speedIncrement = 0.1f;

    private float _blendingTime = 3f;

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

    public void RemoveAllProps()
    {
        PropsInBlender.Clear();
    }

    public void StartBlender()
    {
        IsOn = true;
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

        float H, S, V;
        Color.RGBToHSV(averagedColor, out H, out S, out V);
        H = Mathf.Clamp(H * SpeedValue, 0, 1); //speed value is between 0 and 2 (changing hue)
        S = Mathf.Clamp(S * HeatValue, 0, 1); //heat value is between 0 and 2 (changing saturation)

        Color finalColor = Color.HSVToRGB(H, S, V);

        return finalColor;
    }
}
