using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWorldColor : MonoBehaviour
{
    public void ChangeColor(Color color)
    {
        ColorManager.Instance.ChangeColor(color);
    }
}
