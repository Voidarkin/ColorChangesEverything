using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class ButtonReset : MonoBehaviour, IInteractable
{

    void Start()
    {
        
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
        ColorManager.Instance.ResetColor();
    }

    MeshRenderer m_Renderer;
}
