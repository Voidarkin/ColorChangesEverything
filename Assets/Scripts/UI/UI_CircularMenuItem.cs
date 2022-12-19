using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Animator))]
public class UI_CircularMenuItem : MonoBehaviour
{

    public Color ItemColor;

    void Awake()
    {
        m_Image = GetComponent<Image>();
        m_Anim = GetComponent<Animator>();
    }

    void Start()
    {
        //m_Image = GetComponent<Image>();
        //m_Anim = GetComponent<Animator>();
    }

    public void SetColor(Color color)
    {
        ItemColor = color;
        if(!m_Image) { m_Image = GetComponent<Image>(); }
        m_Image.color = color;
    }

    public void Select()
    {
        m_Anim.SetBool("Selected", true);
    }

    public void Deselect()
    {
        m_Anim.SetBool("Selected", false);
    }

    Image m_Image;
    Animator m_Anim;
}
