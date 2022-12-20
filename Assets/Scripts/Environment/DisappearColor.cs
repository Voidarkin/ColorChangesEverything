using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearColor : MonoBehaviour
{

    void Start()
    {

        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr)
        {
            m_Renderer = mr;
            m_Color = m_Renderer.material.color;
        }

        Collider collider = GetComponent<Collider>();
        if (collider)
        {
            m_Collider = collider;
        }

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if(rigidbody)
        {
            m_Rigidbody = rigidbody;
        }

        ColorManager.Instance.worldColorChange += ColorChanged;

        ColorCanBeChanged ccbc = GetComponent<ColorCanBeChanged>();
        if (ccbc)
        {
            ccbc.colorChange += MaterialColorChanged;
        }

        m_CurrentNode = null;

        m_Active = true;
        m_IsDirty = true;
    }

    public void ColorChanged()
    {   
        m_IsDirty = true;
    }

    void Update()
    {
        if (!m_IsDirty) { return; }

        SetActive();

        SetEnables();

        m_IsDirty = false;
    }

    private void SetActive()
    {
        if (m_CurrentNode)
        {
            m_Active = (m_Color != m_CurrentNode.GetNodeColor()) ? true : false;
        }
        else
        {
            m_Active = (m_Color != ColorManager.Instance.Color) ? true : false;
        }
    }

    private void SetEnables()
    {
        if (m_Renderer)
        {
            m_Renderer.enabled = m_Active;
        }

        if (m_Collider)
        {
            m_Collider.enabled = m_Active;
        }

        if (m_Rigidbody)
        {
            m_Rigidbody.useGravity = m_Active;
            if(!m_Rigidbody.isKinematic)
                m_Rigidbody.velocity = Vector3.zero;
        }
    }

    private void OnEnable()
    {
        ColorChanged();
    }

    public Color GetColor()
    {
        return m_Color;
    }

    public void SetNode(NodeArea na)
    {
        if (na)
        {
            m_CurrentNode = na;
            m_CurrentNode.areaColorChange += ColorChanged;
        }
        m_IsDirty = true;
    }

    public void NodeNull()
    {
        if (m_CurrentNode)
        {
            m_CurrentNode.areaColorChange -= ColorChanged;
        }
        m_CurrentNode = null;
        m_IsDirty = true;
    }

    public NodeArea GetNode()
    {
        return m_CurrentNode;
    }

    public void MaterialColorChanged()
    {
        m_Color = m_Renderer.material.color;
        ColorChanged();
    }

    private void OnDestroy()
    {
        ColorCanBeChanged ccbc = GetComponent<ColorCanBeChanged>();
        if (ccbc)
        {
            ccbc.colorChange -= MaterialColorChanged;
        }
    }

    bool m_Active;
    bool m_IsDirty;

    [SerializeField] NodeArea m_CurrentNode;

    [SerializeField] MeshRenderer m_Renderer;
    [SerializeField] Color m_Color;
    [SerializeField] Rigidbody m_Rigidbody;
    [SerializeField] Collider m_Collider;
}
