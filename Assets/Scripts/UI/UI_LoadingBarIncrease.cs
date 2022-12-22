using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UI_LoadingBarIncrease : MonoBehaviour
{
    void Start()
    {
        m_Slider = GetComponent<Slider>();
        m_Slider.value = 0;
    }

    void Update()
    {
        m_Slider.value += Time.deltaTime;
    }

    Slider m_Slider;
}
