using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Text 변경 스트립트
public class TextChange : MonoBehaviour
{
    EpText epText;
    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private int textIndex;

    private TMP_Text tmpText;
    void Start()
    {
        if (targetObject != null)
        {
            epText = targetObject.GetComponent<EpText>();
            tmpText = GetComponent<TMP_Text>();

            if (epText != null && tmpText != null)
            {
                tmpText.text = epText.epText[textIndex];
            }
        }
    }
}
