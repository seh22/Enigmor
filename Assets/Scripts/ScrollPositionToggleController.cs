using UnityEngine;
using UnityEngine.UI;

public class ScrollPositionToggleController : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Toggle[] toggles;

    // ��ǥ ��ġ�� (localPosition.y ����)
    private float[] targetPositions = { 520f, 1260f, 1800f };
    private int currentIndex = -1;

    void Update()
    {
        float scrollY = scrollRect.content.localPosition.y;

        // ���� ����� ��ġ ã��
        int closestIndex = GetClosestIndex(scrollY);

        if (closestIndex != currentIndex)
        {
            UpdateToggles(closestIndex);
            currentIndex = closestIndex;
        }
    }

    int GetClosestIndex(float currentY)
    {
        float minDistance = Mathf.Abs(currentY - targetPositions[0]);
        int closest = 0;

        for (int i = 1; i < targetPositions.Length; i++)
        {
            float dist = Mathf.Abs(currentY - targetPositions[i]);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = i;
            }
        }

        return closest;
    }

    void UpdateToggles(int activeIndex)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].isOn = (i == activeIndex);
        }
    }
}