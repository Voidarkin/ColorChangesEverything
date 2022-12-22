using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingBarColor : MonoBehaviour
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

        m_Image = GetComponent<Image>();

        int randIndex = Random.Range(0, 5);
        m_Image.color = m_Colors[randIndex];
    }

    void Update()
    {
        
    }

    Image m_Image;
    Color[] m_Colors;
}
