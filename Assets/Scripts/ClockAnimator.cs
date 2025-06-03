using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class ClockAnimator : MonoBehaviour
{
    public Transform hourHand;
    public Transform minuteHand;
    public Transform tickHand; // 10�� ħ
    public Transform secondHand; // ��ħ (�ε巴�� ȸ��)

    [Header("�⺻ �ð� ����")]
    public int setHour = 3;
    public int setMinute = 15;
    public int setSecond = 30;

    private Dictionary<Transform, float> rotationState = new Dictionary<Transform, float>();

    void Start()
    {
        SetInitialRotation(setHour, setMinute, setSecond);

        // ��ħ�� �ε巯�� ���� ȸ��
        AnimateSecondHand();

        // Tick ��� ȸ�� ħ ���
        StartTicking(minuteHand, -6f, 60f);   // ��ħ: 60�ʸ��� 6����
        StartTicking(hourHand, -0.5f, 60f);   // ��ħ: 60�ʸ��� 0.5����
        StartTicking(tickHand, -60f, 10f);    // 10��ħ: 10�ʸ��� 60����
    }

    void SetInitialRotation(int hour, int minute, int second)
    {
        float secAngle = -6f * second;
        float minAngle = -6f * minute;
        float hourAngle = -30f * hour - 0.5f * minute;
        float tickAngle = 0f;

        secondHand.localRotation = Quaternion.Euler(0, 0, secAngle);
        minuteHand.localRotation = Quaternion.Euler(0, 0, minAngle);
        hourHand.localRotation = Quaternion.Euler(0, 0, hourAngle);
        tickHand.localRotation = Quaternion.Euler(0, 0, tickAngle);

        // �ʱ� ȸ���� ���
        rotationState[minuteHand] = minAngle;
        rotationState[hourHand] = hourAngle;
        rotationState[tickHand] = tickAngle;
    }

    void AnimateSecondHand()
    {
        secondHand.DORotate(new Vector3(0, 0, -360f), 60f, RotateMode.LocalAxisAdd)
                  .SetEase(Ease.Linear)
                  .SetLoops(-1);
    }

    /// <summary>
    /// ���� ���ݸ��� ħ�� ���� ������ŭ ƽƽ �����̴� �ִϸ��̼�
    /// </summary>
    void StartTicking(Transform hand, float anglePerTick, float interval)
    {
        if (!rotationState.ContainsKey(hand))
            rotationState[hand] = 0f;

        Sequence tickSeq = DOTween.Sequence();
        tickSeq.AppendCallback(() =>
        {
            rotationState[hand] += anglePerTick;
            hand.DOLocalRotate(new Vector3(0, 0, rotationState[hand]), 0.3f).SetEase(Ease.OutCubic);
        });
        tickSeq.AppendInterval(interval);
        tickSeq.SetLoops(-1);
    }
}
