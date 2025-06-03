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
            UpdateTextColor(toggle.isOn); // 초기 상태 설정
        }
    }

    // `Toggle`의 `isOn` 값이 변경될 때 호출
    void OnToggleValueChanged(bool isOn)
    {
        UpdateTextColor(isOn);
    }

    // 텍스트 색상 업데이트
    void UpdateTextColor(bool isOn)
    {
        if (text != null)
        {
            text.color = isOn ? onColor : offColor;
        }
    }
}
