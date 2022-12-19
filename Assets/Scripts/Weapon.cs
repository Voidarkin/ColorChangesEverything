using UnityEngine;
using Interfaces;

[CreateAssetMenu(menuName = "Scriptable/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Weapon Info")]
    [Tooltip("The weapon's name")]
    public string WeaponName;

    [Tooltip("The weapon's fire rate")]
    public float FireRate = 0.8f;

    [Tooltip("The Weapon Prefab")]
    public GameObject WeaponObject;

    [Header("Colors")]
    [Tooltip("The SelectedColor object")]
    public SelectedColor SelectedColor;

    [Tooltip("The Colors the weapon uses")]
    public Material[] Materials = new Material[3];

    public void Init()
    {
        m_Index = 0;
        m_SelectedColor = Materials[m_Index].color;

        if (m_SelectedColor == Color.white)
        {
            m_SelectedColor = Materials[0].color;
        }

        SelectedColor.ChangeColor(m_SelectedColor);
        SelectedColor.selectedColorChange += SelectedColorChanged;
    }

    public float GetFireRate() { return FireRate; }

    public void CycleSelectedColor()
    {
        //Temporary until UI for selection
        int newIndex = m_Index++;
        newIndex = (newIndex + Materials.Length) % Materials.Length;
        m_Index = newIndex;
        SetSelectedColor(m_Index);
    }

    public void SetSelectedColor(int i)
    {
        if(i < 0 || i >= Materials.Length) { return; }
        m_Index = i;
        m_SelectedColor = Materials[m_Index].color;
        SelectedColor.ChangeColor(Materials[m_Index].color);
    }

    public void SelectedColorChanged()
    {
        m_SelectedColor = SelectedColor.GetColor();
        for(int i = 0; i < Materials.Length; i++)
        {
            if(m_SelectedColor == Materials[i].color)
            {
                m_Index = i;
            }
        }
    }

    public int GetIndex() { return m_Index; }
    public Material GetSelectedMaterial() { return Materials[m_Index]; }
    public Color GetSelectedColor() { return m_SelectedColor; }

    int m_Index;
    Color m_SelectedColor;

}
