using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAdjustments : MonoBehaviour
{

    public SelectedColor SelectedColor;
    public MeshRenderer ColorShown;
    public Transform Muzzle;

    void Start()
    {
        m_IsDirty = true;
        SelectedColor.selectedColorChange += SetDirty;
    }

    void Update()
    {
        if(!m_IsDirty) { return; }

        ColorShown.material.color = SelectedColor.GetColor();

        m_IsDirty = false;
    }

    public Transform GetMuzzle() { return Muzzle; }

    public void SetDirty()
    {
        m_IsDirty = true;
    }

    bool m_IsDirty;
}
