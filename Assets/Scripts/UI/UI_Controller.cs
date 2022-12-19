using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class UI_Controller : MonoBehaviour
{

    void Awake()
    {
        UI_CircularMenu circleMenu = GetComponentInChildren<UI_CircularMenu>();
        if (circleMenu)
        {
            m_CircularMenu = circleMenu;
            m_CircularMenu.gameObject.SetActive(false);
            m_CircularMenuOpen = false;
        }

        UI_Reticle reticle = GetComponentInChildren<UI_Reticle>();
        if (reticle)
        {
            m_Reticle = reticle;
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool IsCircularMenuOpen() { return m_CircularMenuOpen; }

    public void OpenCircularMenu(IController controller, Weapon activeWeapon)
    {
        m_CircularMenu.gameObject.SetActive(true);
        m_CircularMenuOpen = true;
        m_CircularMenu.Init(controller, activeWeapon);

        Time.timeScale = 0.05f;
    }

    public void CloseCircularMenu()
    {
        m_CircularMenu.OnClose();
        m_CircularMenuOpen = false;
        m_CircularMenu.gameObject.SetActive(false);

        Time.timeScale = 1.0f;
    }

    bool m_CircularMenuOpen;
    [SerializeField] UI_CircularMenu m_CircularMenu;
    [SerializeField] UI_Reticle m_Reticle;
    [SerializeField] GameObject m_Health; //HP object will change
}
