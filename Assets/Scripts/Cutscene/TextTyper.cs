using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class TypingEffectRule
{
    public string keyword;
    public float extraDelay = 0.05f;
    public Color color = Color.white;
    public AudioClip sound;
}

public class TextTyper : MonoBehaviour
{
    [SerializeField] private float characterInterval = 0.05f;
    [SerializeField] private bool useRichText = true;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<TypingEffectRule> typingEffects = new();

    private Coroutine typingCoroutine;
    private TMP_Text targetText;
    private string fullTextCache;

    public bool IsTyping { get; private set; } = false;


    public void Skip()
    {
        if (IsTyping && targetText != null)
        {
            StopCoroutine(typingCoroutine);
            targetText.text = fullTextCache;
            if (useRichText)
            {
                targetText.ForceMeshUpdate();
                targetText.maxVisibleCharacters = targetText.textInfo.characterCount;
            }
            IsTyping = false;
        }
    }

    // 새 필드 추가: 키워드 인덱스 기록용
    private Dictionary<int, TypingEffectRule> effectMap;

    public void StartTyping(TMP_Text textComponent, string fullText, float speedMultiplier = 1f)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        targetText = textComponent;
        effectMap = BuildEffectMap(fullText); // 추가: 키워드 위치 기록
        fullTextCache = ApplyRichTextRules(fullText); // richText 처리
        typingCoroutine = StartCoroutine(TypeText(fullTextCache, speedMultiplier));
    }

    private Dictionary<int, TypingEffectRule> BuildEffectMap(string plainText)
    {
        var map = new Dictionary<int, TypingEffectRule>();
        foreach (var rule in typingEffects)
        {
            if (string.IsNullOrEmpty(rule.keyword)) continue;

            int index = 0;
            while ((index = plainText.IndexOf(rule.keyword, index)) != -1)
            {
                for (int i = index; i < index + rule.keyword.Length; i++)
                {
                    if (!map.ContainsKey(i))
                        map[i] = rule;
                }
                index += rule.keyword.Length;
            }
        }
        return map;
    }

    private IEnumerator TypeText(string text, float speedMultiplier)
    {
        IsTyping = true;
        targetText.text = text;
        targetText.ForceMeshUpdate();

        int totalCharacters = targetText.textInfo.characterCount;
        targetText.maxVisibleCharacters = 0;
        float interval = characterInterval / speedMultiplier;

        for (int i = 0; i <= totalCharacters; i++)
        {
            targetText.maxVisibleCharacters = i;

            float delay = interval;

            // original index를 추적할 수 없으므로, 텍스트에서 visibleCharacters 순서대로 매핑
            if (effectMap.TryGetValue(i, out TypingEffectRule rule))
            {
                delay += rule.extraDelay;
                if (rule.sound && audioSource)
                    audioSource.PlayOneShot(rule.sound);
            }

            yield return new WaitForSeconds(delay);
        }

        IsTyping = false;
        typingCoroutine = null;
    }

    private string ExtractWordAtCharIndex(string text, int index)
    {
        if (index < 0 || index >= text.Length || char.IsWhiteSpace(text[index]))
            return "";

        int start = index;
        int end = index;

        while (start > 0 && !char.IsWhiteSpace(text[start - 1]) && text[start - 1] != '<')
            start--;
        while (end < text.Length - 1 && !char.IsWhiteSpace(text[end + 1]) && text[end + 1] != '>')
            end++;

        return text.Substring(start, end - start + 1);
    }

    private string ApplyRichTextRules(string input)
    {
        foreach (var rule in typingEffects)
        {
            if (string.IsNullOrEmpty(rule.keyword)) continue;
            string rich = $"<color=#{ColorUtility.ToHtmlStringRGB(rule.color)}>{rule.keyword}</color>";
            input = input.Replace(rule.keyword, rich);
        }
        return input;
    }
}
