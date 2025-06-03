using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimationController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> animationObjects = new List<GameObject>(); // 애니메이션 오브젝트 리스트
    [SerializeField]
    private float delayBetweenObjects = 0f; // 오브젝트 간 대기 시간
    [SerializeField]
    private bool waitForClick = true; // 클릭 대기 여부

    private int currentIndex = 0;
    private bool isWaitingForClick = false;

    void Start()
    {
        StartCoroutine(PlayAnimations());
    }

    IEnumerator PlayAnimations()
    {
        while (currentIndex < animationObjects.Count)
        {
            GameObject obj = animationObjects[currentIndex];

            if (obj != null)
            {
                obj.SetActive(true); // 현재 오브젝트 활성화
            }

            yield return new WaitForSeconds(delayBetweenObjects); // 일정 시간 대기

            if (waitForClick)
            {
                isWaitingForClick = true;
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                isWaitingForClick = false;
            }

            if (obj != null)
            {
                obj.SetActive(false); // 현재 오브젝트 비활성화
            }

            currentIndex++; // 다음 오브젝트로 이동
        }

        Debug.Log("인트로 애니메이션 종료");
    }

    void Update()
    {
        if (isWaitingForClick && Input.GetMouseButtonDown(0))
        {
            isWaitingForClick = false;
        }
    }
}
