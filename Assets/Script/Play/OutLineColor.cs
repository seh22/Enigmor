using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OutLineColor : MonoBehaviour
{
    private Image outLineColor;

    void Start()
    {
        outLineColor = GetComponent<Image>();
    }

    public void ChangeColor(Color OutLineColor)
    {
        outLineColor.color = OutLineColor;
    }
}
