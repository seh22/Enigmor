using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimationController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> animationObjects = new List<GameObject>(); // �ִϸ��̼� ������Ʈ ����Ʈ
    [SerializeField]
    private float delayBetweenObjects = 0f; // ������Ʈ �� ��� �ð�
    [SerializeField]
    private bool waitForClick = true; // Ŭ�� ��� ����

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
                obj.SetActive(true); // ���� ������Ʈ Ȱ��ȭ
            }

            yield return new WaitForSeconds(delayBetweenObjects); // ���� �ð� ���

            if (waitForClick)
            {
                isWaitingForClick = true;
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                isWaitingForClick = false;
            }

            if (obj != null)
            {
                obj.SetActive(false); // ���� ������Ʈ ��Ȱ��ȭ
            }

            currentIndex++; // ���� ������Ʈ�� �̵�
        }

        Debug.Log("��Ʈ�� �ִϸ��̼� ����");
    }

    void Update()
    {
        if (isWaitingForClick && Input.GetMouseButtonDown(0))
        {
            isWaitingForClick = false;
        }
    }
}
