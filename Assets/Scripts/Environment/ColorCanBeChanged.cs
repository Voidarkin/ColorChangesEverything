using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorCanBeChanged : MonoBehaviour
{

    public delegate void onSelectedColorChange();
    public onSelectedColorChange colorChange;

    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_DefaultMat = m_Renderer.material;
    }

    public void ChangeMaterial(Material mat)
    {
        m_Renderer.material = mat;
        colorChange?.Invoke();
    }

    public Color GetColor()
    {
        return m_Renderer.material.color;
    }

    public void ResetMat()
    {
        m_Renderer.material = m_DefaultMat;
        colorChange?.Invoke();
    }

    MeshRenderer m_Renderer;
    Material m_DefaultMat;
}
