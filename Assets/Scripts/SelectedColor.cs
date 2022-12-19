using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/SelectedColor")]
public class SelectedColor : ScriptableObject
{

    public delegate void onSelectedColorChange();
    public onSelectedColorChange selectedColorChange;

    public void ChangeColor(Color color)
    {
        m_Color = color;
        selectedColorChange?.Invoke();
    }

    public Color GetColor()
    {
        return m_Color;
    }

    [SerializeField] Color m_Color;

}
