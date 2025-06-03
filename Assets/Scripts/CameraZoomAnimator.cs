using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraZoomAnimator : MonoBehaviour
{
    public float targetSize = 9.6f;
    public Vector3 targetPosition = Vector3.zero;
    public float duration = 2f; // 부드럽게 이동할 시간
    // Start is called before the first frame update

    public void CameraMove()
    {
        Camera cam = Camera.main;
        if (cam != null && cam.orthographic)
        {
            // 위치 이동
            cam.transform.DOMove(targetPosition, duration).SetEase(Ease.InOutSine);

            // orthographicSize 확대
            DOTween.To(() => cam.orthographicSize, x => cam.orthographicSize = x, targetSize, duration)
                   .SetEase(Ease.InOutSine);
        }
        else
        {
            Debug.LogWarning("메인 카메라가 없거나 Orthographic 모드가 아닙니다.");
        }
    }

}
