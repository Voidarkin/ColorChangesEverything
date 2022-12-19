using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class NodeArea : MonoBehaviour
{

    public delegate void onSelectedColorChange();
    public onSelectedColorChange areaColorChange;

    private void Awake()
    {
        m_Active = false;

        m_Renderer = GetComponent<MeshRenderer>();
        m_Color = m_Renderer.material.color;
        m_DefaultColor = m_Color;

        m_Collider = GetComponent<Collider>();

        m_Volume = GetComponentInChildren<Volume>();
        if (m_Volume)
        {
            if (m_Volume.profile.TryGet<ColorAdjustments>(out m_ColorAdjustments))
            {
                m_ColorAdjustments.colorFilter.overrideState = true;
                m_ColorAdjustments.colorFilter.value = m_Color;
            }
        }

        m_IsDirty = true;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!m_IsDirty) { return; }

        m_Renderer.enabled = m_Active;
        m_Collider.enabled = m_Active;

        if (m_Volume)
        {
            if(m_ColorAdjustments)
            {
                m_ColorAdjustments.colorFilter.overrideState = true;
                m_ColorAdjustments.colorFilter.value = m_Color;
            }
            m_Volume.enabled = m_Active;
        }

        m_IsDirty = false;
    }

    public void Activate()
    {
        m_Active = true;
        SetDirty();
    }

    public void Deactivate()
    {
        m_Active = false;
        SetDirty();
    }

    public void SetDirty() { m_IsDirty = true; }

    public void ChangeColor(Color color)
    {
        if(m_Renderer == null)
        {
            m_Renderer = GetComponent<MeshRenderer>();
        }

        m_Color = color;
        Color alphaChange = m_Color;
        alphaChange.a = 0.3f;

        m_Renderer.material.color = alphaChange;

        areaColorChange?.Invoke();
    }

    public Color GetNodeColor()
    {
        return m_Color;
    }

    private void OnTriggerEnter(Collider other)
    {
        DisappearColor dc = other.GetComponent<DisappearColor>();
        if (dc)
        {
            if (dc.GetNode() == this) { return; }

            dc.SetNode(this);
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        DisappearColor dc = other.GetComponent<DisappearColor>();
        if (dc)
        {
            if(dc.GetNode() == this) { return; }

            dc.SetNode(this);
        }
    }
    */

    private void OnTriggerExit(Collider other)
    {
        DisappearColor dc = other.GetComponent<DisappearColor>();
        if (dc)
        {
            if (dc.GetNode() != this) { return; }

            dc.NodeNull();
        }
    }

    bool m_Active;
    bool m_IsDirty;

    [SerializeField] Volume m_Volume;
    [SerializeField] ColorAdjustments m_ColorAdjustments;

    MeshRenderer m_Renderer;
    [SerializeField] Color m_Color;
    Collider m_Collider;
    Color m_DefaultColor;
}
