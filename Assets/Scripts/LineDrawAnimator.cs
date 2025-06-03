using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawAnimator_DOTween : MonoBehaviour
{
    public Transform[] pointTransforms; // ���� �� ������Ʈ��
    public float drawDuration = 5f;     // ��ü �׸��� �ð�

    private LineRenderer lineRenderer;



    public void PlayLineAnimation()
    {
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(AnimateLineWithDOTween());
    }

    IEnumerator AnimateLineWithDOTween()
    {
        int pointCount = pointTransforms.Length;
        if (pointCount == 0) yield break;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, pointTransforms[0].position);

        float interval = drawDuration / (pointCount - 1);

        for (int i = 1; i < pointCount; i++)
        {
            int currentIndex = i;
            lineRenderer.positionCount = currentIndex + 1;

            Vector3 start = pointTransforms[currentIndex - 1].position;
            Vector3 end = pointTransforms[currentIndex].position;
            Vector3 current = start;

            // ���� ���� ���� �׷����� ȿ�� (����)
            float elapsed = 0f;
            while (elapsed < interval)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / interval);
                current = Vector3.Lerp(start, end, t);
                lineRenderer.SetPosition(currentIndex, current);
                yield return null;
            }

            lineRenderer.SetPosition(currentIndex, end); // ���� ��ġ ����
        }
    }
}
