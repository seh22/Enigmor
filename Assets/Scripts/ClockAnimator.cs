using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class ClockAnimator : MonoBehaviour
{
    public Transform hourHand;
    public Transform minuteHand;
    public Transform tickHand; // 10초 침
    public Transform secondHand; // 초침 (부드럽게 회전)

    [Header("기본 시간 설정")]
    public int setHour = 3;
    public int setMinute = 15;
    public int setSecond = 30;

    private Dictionary<Transform, float> rotationState = new Dictionary<Transform, float>();

    void Start()
    {
        SetInitialRotation(setHour, setMinute, setSecond);

        // 초침은 부드러운 연속 회전
        AnimateSecondHand();

        // Tick 기반 회전 침 등록
        StartTicking(minuteHand, -6f, 60f);   // 분침: 60초마다 6도씩
        StartTicking(hourHand, -0.5f, 60f);   // 시침: 60초마다 0.5도씩
        StartTicking(tickHand, -60f, 10f);    // 10초침: 10초마다 60도씩
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

        // 초기 회전값 등록
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
    /// 일정 간격마다 침을 일정 각도만큼 틱틱 움직이는 애니메이션
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
