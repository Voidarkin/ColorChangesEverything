using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

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
    }

    void Update()
    {
        
    }

    public void ColorChanged()
    {
        if (m_ColorCanBeChanged.GetColor() != Color.white)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
        ChangeColor(m_ColorCanBeChanged.GetColor());
    }

    public void ChangeColor(Color color)
    {
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

    ColorCanBeChanged m_ColorCanBeChanged;

}
