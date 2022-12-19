using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class Button : MonoBehaviour, IInteractable
{

    public GameObject LinkedColorPlate;

    void Start()
    {
        if (!LinkedColorPlate) { return; }

        MeshRenderer mr = LinkedColorPlate.GetComponent<MeshRenderer>();
        if (mr)
        {
            m_Renderer = mr;
        }

    }

    public void DisplayInteraction()
    {
        throw new System.NotImplementedException();
    }

    public void HideInteraction()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        ChangeWorldColor cwc = GetComponent<ChangeWorldColor>();
        if (cwc)
        {
            cwc.ChangeColor(m_Renderer.material.color);
        }

    }

    MeshRenderer m_Renderer;
}