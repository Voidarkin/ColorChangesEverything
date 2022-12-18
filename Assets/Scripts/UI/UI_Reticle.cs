using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_Reticle : MonoBehaviour
{

    public SelectedColor SelectedColor;

    void Start()
    {
        SelectedColor.selectedColorChange += SetDirty;
        Image image = GetComponent<Image>();
        if (image)
        {
            m_Reticle = image;
        }
    }

    void Update()
    {
        if (!m_IsDirty) { return; }

        m_Reticle.color = SelectedColor.GetColor();
    }

    public void SetDirty()
    {
        m_IsDirty = true;
    }

    Image m_Reticle;
    bool m_IsDirty;
}
