using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorCanBeChanged : MonoBehaviour
{

    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_DefaultMat = m_Renderer.material;
    }

    public void ChangeMaterial(Material mat)
    {
        m_Renderer.material = mat;
    }

    MeshRenderer m_Renderer;
    Material m_DefaultMat;
}
