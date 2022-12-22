using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_RandomButtonColor : MonoBehaviour
{

    void Start()
    {
        m_Colors = new Color[6];

        m_Colors[0] = Color.red;
        m_Colors[1] = ExpandedColors.orange;
        m_Colors[2] = Color.yellow;
        m_Colors[3] = Color.green;
        m_Colors[4] = Color.blue;
        m_Colors[5] = ExpandedColors.purple;

        m_Button = GetComponent<Button>();
        ChangeHoverColor();
    }

    public void ChangeHoverColor()
    {
        int randIndex = Random.Range(0, m_Colors.Length);
        ColorBlock cb = m_Button.colors;
        cb.highlightedColor = m_Colors[randIndex];
        m_Button.colors = cb;
    }

    Color[] m_Colors;
    Button m_Button;
}
