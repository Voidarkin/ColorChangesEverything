using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public InteractButton AttachedButton;
    public GameObject Area;
    public NodeArea EffectedArea;
   
    void Start()
    {
        ColorCanBeChanged ccbc = GetComponent<ColorCanBeChanged>();
        if (ccbc)
        {
            m_ColorCanBeChanged = ccbc;
            m_ColorCanBeChanged.colorChange += ColorChanged;
        }

        if (AttachedButton)
        {
            AttachedButton.buttonPressed += ButtonPressed;
        }
    }

    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        GameObject linkedColor = AttachedButton.LinkedColorPlate;
        MeshRenderer linkedColorMesh = linkedColor.GetComponent<MeshRenderer>();
        if (linkedColorMesh)
        {
            Color color = linkedColorMesh.material.color;

            MeshRenderer nodeRenderer = GetComponent<MeshRenderer>();
            if (nodeRenderer)
            {
                nodeRenderer.material.color = color;
            }

            ChangeColor(color);
        }
    }

    public void ColorChanged()
    {
        if(!AttachedButton)
            ChangeColor(m_ColorCanBeChanged.GetColor());
    }

    public void ChangeColor(Color color)
    {
        if (color != Color.white)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
        EffectedArea.ChangeColor(color);
    }

    public void Activate()
    {
        Area.SetActive(true);
        EffectedArea.Activate();
    }

    public void Deactivate()
    {
        EffectedArea.Deactivate();
        Area.SetActive(false);
    }

    public void ResetNode()
    {
        Deactivate();
    }

    private void OnDestroy()
    {
        if (AttachedButton)
        {
            AttachedButton.buttonPressed -= ButtonPressed;
        }
    }

    ColorCanBeChanged m_ColorCanBeChanged;

}
