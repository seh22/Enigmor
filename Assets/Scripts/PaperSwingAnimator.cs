using UnityEngine;
using DG.Tweening;

public class PaperSwingAnimator: MonoBehaviour
{
    public float initialIntensity = 15f;       // ÃÊ±â Èçµé¸² °¢µµ
    public int oscillationCount = 5;           // ¿Õº¹ È½¼ö
    public float duration = 3f;                // ÃÑ ½Ã°£

    public void PlayDampedSwing()
    {
        transform.rotation = Quaternion.identity;

        float singleDuration = duration / (oscillationCount * 2f);
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < oscillationCount; i++)
        {
            float damping = Mathf.Lerp(initialIntensity, 0, (float)i / oscillationCount); // Á¡Á¡ ÁÙ¾îµê
            seq.Append(transform.DORotate(new Vector3(0, 0, damping), singleDuration).SetEase(Ease.InOutSine));
            seq.Append(transform.DORotate(new Vector3(0, 0, -damping), singleDuration).SetEase(Ease.InOutSine));
        }

        seq.Append(transform.DORotate(Vector3.zero, singleDuration).SetEase(Ease.OutSine));

        Debug.Log("swing");
    }

}
