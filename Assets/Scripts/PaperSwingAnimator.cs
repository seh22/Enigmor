using UnityEngine;
using DG.Tweening;

public class PaperSwingAnimator: MonoBehaviour
{
    public float initialIntensity = 15f;       // �ʱ� ��鸲 ����
    public int oscillationCount = 5;           // �պ� Ƚ��
    public float duration = 3f;                // �� �ð�

    public void PlayDampedSwing()
    {
        transform.rotation = Quaternion.identity;

        float singleDuration = duration / (oscillationCount * 2f);
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < oscillationCount; i++)
        {
            float damping = Mathf.Lerp(initialIntensity, 0, (float)i / oscillationCount); // ���� �پ��
            seq.Append(transform.DORotate(new Vector3(0, 0, damping), singleDuration).SetEase(Ease.InOutSine));
            seq.Append(transform.DORotate(new Vector3(0, 0, -damping), singleDuration).SetEase(Ease.InOutSine));
        }

        seq.Append(transform.DORotate(Vector3.zero, singleDuration).SetEase(Ease.OutSine));

        Debug.Log("swing");
    }

}
