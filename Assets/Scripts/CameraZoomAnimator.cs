using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraZoomAnimator : MonoBehaviour
{
    public float targetSize = 9.6f;
    public Vector3 targetPosition = Vector3.zero;
    public float duration = 2f; // �ε巴�� �̵��� �ð�
    // Start is called before the first frame update

    public void CameraMove()
    {
        Camera cam = Camera.main;
        if (cam != null && cam.orthographic)
        {
            // ��ġ �̵�
            cam.transform.DOMove(targetPosition, duration).SetEase(Ease.InOutSine);

            // orthographicSize Ȯ��
            DOTween.To(() => cam.orthographicSize, x => cam.orthographicSize = x, targetSize, duration)
                   .SetEase(Ease.InOutSine);
        }
        else
        {
            Debug.LogWarning("���� ī�޶� ���ų� Orthographic ��尡 �ƴմϴ�.");
        }
    }

}
