using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputBlockerByImage : MonoBehaviour
{
    [SerializeField] private Image blockingImage;

    /// <summary>
    /// ���� ���콺�� ��ġ�� UI ���Ŀ �̹��� ���� �ִ��� Ȯ��
    /// </summary>
    public bool IsBlocked()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return blockingImage != null && blockingImage.raycastTarget &&
               EventSystem.current.IsPointerOverGameObject();
#elif UNITY_IOS || UNITY_ANDROID
        if (blockingImage == null || !blockingImage.raycastTarget || EventSystem.current == null)
            return false;

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                return true;
        }
        return false;
#else
        return false;
#endif
    }
}
