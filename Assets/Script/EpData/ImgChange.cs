using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//img 변경을 위한 스크립트
public class ImgChange : MonoBehaviour
{
    EpImg epImg;
    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private int imgIndex;

    private Image ImgSprite;
    void Start()
    {
        if (targetObject != null)
        {
            epImg = targetObject.GetComponent<EpImg>();
            ImgSprite = GetComponent<Image>();

            if (epImg != null && ImgSprite != null)
            {
                ImgSprite.sprite = epImg.epImage[imgIndex];
            }
        }
    }
}
