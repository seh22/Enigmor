using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TogglePositionChanger : MonoBehaviour
{
    private Transform targetObject;
    public Vector3 targetPosition;
    public float moveDuration = 0.5f;

    private Vector3 originalPosition;

    void Awake()
    {
        targetObject = GetComponent<Transform>();
        originalPosition = targetObject.position;
    }

    public void OnToggleChanged(bool isOn)
    {
        
        if (isOn)
        {
            // true일 때 targetPosition으로 이동
            targetObject.DOMove(targetPosition, moveDuration).SetEase(Ease.OutQuad);
        }
        else
        {
            // false일 때 원래 위치로 되돌아감
            targetObject.DOMove(originalPosition, moveDuration);
        }
    }
}