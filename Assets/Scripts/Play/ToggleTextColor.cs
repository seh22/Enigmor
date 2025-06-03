using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleTextColor : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Text text;

    public Color onColor;
    public Color offColor;

    void Start()
    {
        if (toggle != null && text != null)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
            UpdateTextColor(toggle.isOn); // �ʱ� ���� ����
        }
    }

    // `Toggle`�� `isOn` ���� ����� �� ȣ��
    void OnToggleValueChanged(bool isOn)
    {
        UpdateTextColor(isOn);
    }

    // �ؽ�Ʈ ���� ������Ʈ
    void UpdateTextColor(bool isOn)
    {
        if (text != null)
        {
            text.color = isOn ? onColor : offColor;
        }
    }
}
