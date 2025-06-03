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
            // true�� �� targetPosition���� �̵�
            targetObject.DOMove(targetPosition, moveDuration).SetEase(Ease.OutQuad);
        }
        else
        {
            // false�� �� ���� ��ġ�� �ǵ��ư�
            targetObject.DOMove(originalPosition, moveDuration);
        }
    }
}