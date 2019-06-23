using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
public class ShapeHighlighterController : HighlighterController
{
    public float speed = 200f;

    private readonly int period = 1530;
    private float counter = 0f;


    //选中描边高亮

    protected struct Preset
    {
        public string name;
        public int downsampleFactor;
        public int iterations;
        public float blurMinSpread;
        public float blurSpread;
        public float blurIntensity;
    }

    List<Preset> presets = new List<Preset>()
    {
        new Preset() { name = "Default",    downsampleFactor = 4,   iterations = 2, blurMinSpread = 0.65f,  blurSpread = 0.25f, blurIntensity = 0.3f },
        new Preset() { name = "Strong",     downsampleFactor = 4,   iterations = 2, blurMinSpread = 0.5f,   blurSpread = 0.15f, blurIntensity = 0.325f },
        new Preset() { name = "Wide",       downsampleFactor = 4,   iterations = 4, blurMinSpread = 0.5f,   blurSpread = 0.15f, blurIntensity = 0.325f },
        new Preset() { name = "Speed",      downsampleFactor = 4,   iterations = 1, blurMinSpread = 0.75f,  blurSpread = 0f,    blurIntensity = 0.35f },
        new Preset() { name = "Quality",    downsampleFactor = 2,   iterations = 3, blurMinSpread = 0.5f,   blurSpread = 0.5f,  blurIntensity = 0.28f },
        new Preset() { name = "Solid 1px",  downsampleFactor = 1,   iterations = 1, blurMinSpread = 1f,     blurSpread = 0f,    blurIntensity = 1f },
        new Preset() { name = "Solid 2px",  downsampleFactor = 1,   iterations = 2, blurMinSpread = 1f,     blurSpread = 0f,    blurIntensity = 1f },
        new Preset() { name = "Solid 6px",  downsampleFactor = 1,   iterations = 6, blurMinSpread = 1f,     blurSpread = 0f,    blurIntensity = 1f }
    };
    //
    private void Awake()
    {
        base.Awake();
        h.ConstantOnImmediate(Color.red);
        SetPresetSettings(AppSceneBase.main.mainCamera, presets[7]);
    }
    // private void Start()
    // {
    //     
    // }
    //  

    // Some color spectrum magic
    float GetColorValue(int offset, int x)
    {
        int o = 0;
        x = (x - offset) % period;
        if (x < 0) { x += period; }
        if (x < 255) { o = x; }
        if (x >= 255 && x < 765) { o = 255; }
        if (x >= 765 && x < 1020) { o = 1020 - x; }
        return (float)o / 255f;
    }

    void SetPresetSettings(Camera cam, Preset p)
    {
        HighlightingBase hb = cam.GetComponent<HighlightingBase>();//FindObjectOfType<HighlightingBase>();
        Debug.Log("SetPresetSettings");
        if (hb == null)
        {
            Debug.Log("SetPresetSettings is null");
            return;
        }

        hb.downsampleFactor = p.downsampleFactor;
        hb.iterations = p.iterations;
        hb.blurMinSpread = p.blurMinSpread;
        hb.blurSpread = p.blurSpread;
        hb.blurIntensity = p.blurIntensity;
    }
    public void UpdateColor(Color cr)
    {
        h.ConstantOnImmediate(cr);
    }
}
