using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color Color { get; private set; }
    public VolumeProfile Profile;

    public delegate void onColorChange();
    public onColorChange worldColorChange;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Color = Color.white;

        ColorAdjustments colorAdjustments;
        if (Profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            m_ColorAdjustments = colorAdjustments;
            m_ColorAdjustments.colorFilter.overrideState = true;
            m_ColorAdjustments.colorFilter.value = Color;
        }

        m_IsDirty = false;
    }

    void Update()
    {
        if (!m_IsDirty) { return; }

        if (!m_ColorAdjustments) { return; }

        m_ColorAdjustments.colorFilter.overrideState = true;
        m_ColorAdjustments.colorFilter.value = Color;

        m_IsDirty = false;
    }

    public void ChangeColor(Color color)
    {
        if(Color == color) { return; }

        Color = color;
        worldColorChange?.Invoke();
        m_IsDirty = true;
    }

    public bool IsActiveColor(Color color)
    {
        return (Color == color);
    }

    public void ResetColor()
    {
        ChangeColor(Color.white);

        ColorCanBeChanged[] objects = FindObjectsOfType<ColorCanBeChanged>();
        foreach(ColorCanBeChanged ccbc in objects)
        {
            ccbc.ResetMat();
        }

        Node[] nodes = FindObjectsOfType<Node>();
        foreach (Node node in nodes)
        {
            node.ResetNode();
        }
    }

    void OnDestroy()
    {
        if (!m_ColorAdjustments) { return; }

        m_ColorAdjustments.colorFilter.overrideState = true;
        m_ColorAdjustments.colorFilter.value = Color.white;
    }

    ColorAdjustments m_ColorAdjustments;

    bool m_IsDirty;
}
