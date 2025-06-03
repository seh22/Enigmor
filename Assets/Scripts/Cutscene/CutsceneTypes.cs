using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class ImageSlot
{
    public GameObject root;

    [HideInInspector] public Image image;
    [HideInInspector] public RectTransform rect;

    public void CacheComponents()
    {
        if (root != null)
        {
            image = root.GetComponentInChildren<Image>();
            rect = root.GetComponent<RectTransform>();
        }
    }

    public void Apply(Sprite sprite, Vector2 size, Vector2 pos, bool active = true)
    {
        if (image != null)
        {
            image.sprite = sprite;
            image.color = new Color(1, 1, 1, 1);
        }

        if (rect != null)
        {
            rect.sizeDelta = size;
            rect.anchoredPosition = pos;
        }

        if (root != null)
            root.SetActive(active && sprite != null); 
    }

    public void Clear()
    {
        if (image != null)
            image.sprite = null;

        if (root != null)
            root.SetActive(false);
    }

    public void FadeIn(float duration = 0.5f)
    {
        if (root != null) root.SetActive(true);
        if (image != null)
        {
            image.color = new Color(1, 1, 1, 0);
            image.DOFade(1f, duration);
        }
    }

    public void FadeOut(float duration = 0.5f, bool deactivateAfter = true)
    {
        if (image != null)
        {
            image.DOFade(0f, duration).OnComplete(() =>
            {
                if (deactivateAfter && root != null)
                    root.SetActive(false);
            });
        }
    }

    public void Pop(float duration = 0.3f, float scale = 1.1f)
    {
        if (rect != null)
        {
            rect.localScale = Vector3.zero;
            rect.DOScale(scale, duration).SetEase(Ease.OutBack);
        }
    }

    public void Flash(float flashAlpha = 1f, float duration = 0.1f)
    {
        if (image == null) return;

        Color original = image.color;
        image.color = new Color(original.r, original.g, original.b, 0f);
        image.DOFade(flashAlpha, duration / 2f).OnComplete(() =>
        {
            image.DOFade(0f, duration / 2f).OnComplete(() =>
            {
                image.color = original;
            });
        });
    }
}
