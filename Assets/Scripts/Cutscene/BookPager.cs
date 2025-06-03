using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class BookPager : MonoBehaviour
{

    public CanvasGroup[] pageGroups; // �������� CanvasGroup
    public Button nextButton;
    public Button prevButton;
    private int currentIndex = 0;
    private float fadeDuration = 0.5f;

    private void Start()
    {
        // ��� ������ ����� ���� �������� ������
        for (int i = 0; i < pageGroups.Length; i++)
        {
            pageGroups[i].alpha = (i == currentIndex) ? 1f : 0f;
            pageGroups[i].interactable = (i == currentIndex);
            pageGroups[i].blocksRaycasts = (i == currentIndex);
        }

        UpdateButtons();

        nextButton.onClick.AddListener(() => TurnPage(1));
        prevButton.onClick.AddListener(() => TurnPage(-1));
    }

    private void TurnPage(int direction)
    {
        int nextIndex = currentIndex + direction;
        if (nextIndex < 0 || nextIndex >= pageGroups.Length) return;

        // ���� ������ ����
        pageGroups[currentIndex].DOFade(0f, fadeDuration).OnComplete(() =>
        {
            pageGroups[currentIndex].interactable = false;
            pageGroups[currentIndex].blocksRaycasts = false;
        });

        // ���� ������ ǥ��
        pageGroups[nextIndex].DOFade(1f, fadeDuration).OnStart(() =>
        {
            pageGroups[nextIndex].interactable = true;
            pageGroups[nextIndex].blocksRaycasts = true;
        });

        currentIndex = nextIndex;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < pageGroups.Length - 1;
    }

}
