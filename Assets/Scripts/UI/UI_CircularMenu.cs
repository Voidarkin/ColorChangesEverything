using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class UI_CircularMenu : MonoBehaviour
{

    public GameObject MenuItemPrefab;
    public SelectedColor SelectedColor;
    public GameObject Pointer;

    void Start()
    {
    }

    public void Init(IController controller, Weapon activeWeapon)
    {
        if(controller == null) { return; }
        if(!activeWeapon) { return; }

        m_Controller = controller;

        if(m_NumMenuItems != 0)
            ClearMenuItems();

        m_NumMenuItems = activeWeapon.Materials.Length;
        m_MenuItems = new GameObject[m_NumMenuItems];
        int angle = 360 / m_NumMenuItems;

        for(int i = 0; i < m_NumMenuItems; i++)
        {
            m_MenuItems[i] = Instantiate(MenuItemPrefab, transform);

            UI_CircularMenuItem item = m_MenuItems[i].GetComponentInChildren<UI_CircularMenuItem>();
            Color color = activeWeapon.Materials[i].color;
            item.SetColor(color);

            Vector3 rot = m_MenuItems[i].transform.rotation.eulerAngles;
            rot.z = angle * i;
            m_MenuItems[i].transform.rotation = Quaternion.Euler(rot);

        }

        m_CurrentSelection = activeWeapon.GetIndex();
        m_CurrentMenuItem = m_MenuItems[m_CurrentSelection].GetComponentInChildren<UI_CircularMenuItem>();
        m_CurrentMenuItem.Select();
    }

    void ClearMenuItems()
    {
        System.Array.Clear(m_MenuItems, 0, m_NumMenuItems);
        m_NumMenuItems = 0;
    }

    void Update()
    {
        //m_NormalisedMousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);

        Vector3 lookInput = m_Controller.GetLookInput();

        float mouseX = lookInput.y;
        float mouseY = -lookInput.x;

        m_NormalisedMousePosition = new Vector2(mouseX, mouseY);

        float cutoff = 0.2f;
        if ((m_NormalisedMousePosition.x < cutoff && m_NormalisedMousePosition.x > -cutoff) &&
            (m_NormalisedMousePosition.y < cutoff && m_NormalisedMousePosition.y > -cutoff)) { return; }

        m_CurrentAngle = Mathf.Atan2(m_NormalisedMousePosition.y, m_NormalisedMousePosition.x) * Mathf.Rad2Deg;

        m_CurrentAngle = (m_CurrentAngle + 360) % 360;

        Vector3 pointerRot = new Vector3(0, 0, m_CurrentAngle);
        Pointer.transform.rotation = Quaternion.Euler(pointerRot);

        m_CurrentSelection = (int)m_CurrentAngle / (360 / m_NumMenuItems);

        if(m_CurrentSelection != m_PreviousSelection)
        {
            m_PreviousMenuItem = m_MenuItems[m_PreviousSelection].GetComponentInChildren<UI_CircularMenuItem>();
            m_PreviousMenuItem.Deselect();
            m_PreviousSelection = m_CurrentSelection;

            m_CurrentMenuItem = m_MenuItems[m_CurrentSelection].GetComponentInChildren<UI_CircularMenuItem>();
            m_CurrentMenuItem.Select();
        }
    }

    public void OnClose()
    {
        if(m_CurrentMenuItem == null) { return; }
        Color color = m_CurrentMenuItem.ItemColor;

        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject != Pointer)
            {
                Destroy(transform.GetChild(i).gameObject);
            } 
        }

        SelectedColor.ChangeColor(color);
    }

    IController m_Controller;
    int m_NumMenuItems = 0;
    [SerializeField] Vector2 m_NormalisedMousePosition;
    [SerializeField] float m_CurrentAngle;
    [SerializeField] int m_CurrentSelection;
    int m_PreviousSelection;
    GameObject[] m_MenuItems;

    UI_CircularMenuItem m_CurrentMenuItem;
    UI_CircularMenuItem m_PreviousMenuItem;
}
