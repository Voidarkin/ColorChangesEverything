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

        ColorManager.Instance.colorChange += ColorChanged;

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

        if (m_Color == ColorManager.Instance.Color)
        {
            m_Active = false;
        }
        else
        {
            m_Active = true;
        }

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
        }

        m_IsDirty = false;
    }

    private void OnEnable()
    {
        ColorChanged();
    }

    bool m_Active;
    bool m_IsDirty;

    [SerializeField] MeshRenderer m_Renderer;
    [SerializeField] Color m_Color;
    [SerializeField] Rigidbody m_Rigidbody;
    [SerializeField] Collider m_Collider;
}
