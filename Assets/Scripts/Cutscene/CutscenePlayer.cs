using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CutscenePlayer : MonoBehaviour
{

    [Header("UI 연결")]
    public Image fadeOverlayImage;
    public Image backgroundImage;
    public TMP_Text characterDialogueText;
    public TMP_Text narrationText;
    public Image characterImageSlot;
    public TMP_Text characterNameText;
    public Button skipButton;


    [Header("이미지 슬롯 그룹")]
    public ImageSlot[] imageSlots;

    [Header("UI 루트 그룹")]
    public GameObject characterUIRoot;
    public GameObject narrationUIRoot;

    [Header("컷씬 데이터")]
    public CutsceneSequence sequence;

    private bool clickReceived = false;
    private Coroutine playingCoroutine;
    private float currentSpeed = 1f;
    private TextTyper textTyper;

    private Dictionary<string, GameObject> keywordObjectMap = new();

    [Header("프리팹 루트")]
    public GameObject spawnRoot;

    [Header("타이틀 텍스트 오브젝트")]
    public GameObject topbarTitleTextObject;
    public GameObject midTitleTextObject;

    [Header("UI 생성 루트")]
    public Transform uiRootTransform;

    void Awake()
    {
        

        for (int i = 0; i < imageSlots.Length; i++)
        {
            imageSlots[i].CacheComponents();

            if (imageSlots[i].root == null)
            {
                Debug.LogWarning($"[컷씬] imageSlots[{i}].root is null!");
                continue;
            }

            keywordObjectMap[$"slot{i}"] = imageSlots[i].root;
            Debug.Log($"[컷씬] slot{i} mapped to {imageSlots[i].root.name}");
        }

        keywordObjectMap["characterUI"] = characterUIRoot;
        keywordObjectMap["narrationUI"] = narrationUIRoot;
        keywordObjectMap["background"] = backgroundImage?.gameObject;
        keywordObjectMap["spawnRoot"] = spawnRoot;
        keywordObjectMap["topbarTitleText"] = topbarTitleTextObject; // TMP_Text
        keywordObjectMap["midTitleText"] = midTitleTextObject;       // TMP_Text



    }

    void Start()
    {
        textTyper = GetComponent<TextTyper>();
        if (skipButton != null)
            skipButton.onClick.AddListener(Skip);

        

        Play();
    }

    public void Play()
    {
        if (playingCoroutine != null)
            StopCoroutine(playingCoroutine);

        playingCoroutine = StartCoroutine(PlaySequence());
    }

    public void Skip()
    {
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
            clickReceived = true;
            EndCutscene();
        }
    }

    private IEnumerator PlaySequence()
    {
        if (fadeOverlayImage != null)
        {
            Color c = fadeOverlayImage.color;
            fadeOverlayImage.color = new Color(c.r, c.g, c.b, 1f);
            yield return fadeOverlayImage.DOFade(0f, 1f).WaitForCompletion();
        }

        foreach (var action in sequence.actions)
        {
            if (action.activationTiming == ObjectActivationTiming.BeforeAction &&
        IsKeywordControllableType(action.type))
            {
                SetActiveByKeyword(action.targetKeyword, action.setActive);
            }

            if (action.delayBefore > 0)
                yield return new WaitForSeconds(action.delayBefore);

            switch (action.type)
            {
                case ActionType.CharacterSpeak:
                    characterUIRoot?.SetActive(true);
                    narrationUIRoot?.SetActive(false);
                    ApplyCharacter(action);
                    characterNameText.text = action.characterName;
                    if (action.dialogueTexts != null && action.dialogueTexts.Length > 0)
                        yield return PlayDialogueArray(action.dialogueTexts, characterDialogueText);
                    break;

                case ActionType.NarrationOnly:
                    characterUIRoot?.SetActive(false);
                    narrationUIRoot?.SetActive(true);
                    if (action.narrationTexts != null && action.narrationTexts.Length > 0)
                        yield return PlayDialogueArray(action.narrationTexts, narrationText);
                    break;

                case ActionType.ShowImage:
                    int idx = Mathf.Clamp(action.imageSlotIndex, 0, imageSlots.Length - 1);
                    imageSlots[idx].Apply(action.imageSprite, action.imageSize, action.imagePosition);
                    imageSlots[idx].FadeIn();

                    break;

                case ActionType.WaitForClick:
                    yield return WaitForUserClick();
                    break;

                case ActionType.Wait:
                    yield return new WaitForSeconds(action.duration);
                    break;

                case ActionType.Fade:
                    float targetAlpha = (action.fadeMode == FadeMode.FadeIn) ? 0f : 1f;
                    float fromAlpha = 1f - targetAlpha;

                    if (fadeOverlayImage != null)
                    {
                        Color c = fadeOverlayImage.color;
                        fadeOverlayImage.color = new Color(c.r, c.g, c.b, fromAlpha);

                        Tween tween = fadeOverlayImage.DOFade(targetAlpha, action.duration)
                            .SetEase(Ease.Linear);

                        yield return tween.WaitForCompletion();
                    }
                    break;

                case ActionType.BackgroundChange:
                    if (backgroundImage != null)
                        backgroundImage.sprite = action.backgroundSprite;
                    break;

                case ActionType.ChangeColor:
                    {
                        if (keywordObjectMap.TryGetValue(action.colorTargetKeyword, out GameObject obj))
                        {
                            Image image = obj.GetComponentInChildren<Image>();
                            if (image != null)
                            {
                                if (action.useFade)
                                {
                                    image.DOColor(action.targetColor, action.colorChangeDuration);
                                    yield return new WaitForSeconds(action.colorChangeDuration);
                                }
                                else
                                {
                                    image.color = action.targetColor;
                                }

                            }
                        }

                        else
                        {
                            Debug.LogWarning($"[컷씬] ChangeColor: 키워드 '{action.colorTargetKeyword}'에 해당하는 오브젝트가 없습니다.");
                        }
                        break;
                    }
                case ActionType.SpawnPrefab:
                    {
                        if (action.prefabToSpawn != null)
                        {
                            GameObject parent = null;
                            keywordObjectMap.TryGetValue(action.parentKeyword, out parent);

                            GameObject instance = Instantiate(action.prefabToSpawn, parent?.transform ?? null);
                            instance.transform.localPosition = action.localPosition;
                            instance.transform.localScale = action.localScale;

                            if (action.deactivateAfterSpawn)
                                instance.SetActive(false);

                            if (!string.IsNullOrEmpty(action.targetKeyword))
                                keywordObjectMap[action.targetKeyword] = instance;
                        }
                        else
                        {
                            Debug.LogWarning($"[컷씬] SpawnPrefab: 프리팹이 지정되지 않았습니다 ({action.actionName})");
                        }
                        break;
                    }
                case ActionType.DespawnPrefab:
                    {
                        if (keywordObjectMap.TryGetValue(action.despawnTargetKeyword, out GameObject go))
                        {
                            if (go != null)
                            {
                                if (action.immediateDestroy)
                                    Destroy(go);
                                else
                                    go.SetActive(false);
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"[컷씬] DespawnPrefab: 키워드 '{action.despawnTargetKeyword}'에 해당하는 오브젝트가 없습니다.");
                        }
                        break;
                    }
                case ActionType.SetTitleWithAnimation:
                    {
                        string newTitleText = action.topbarText;

                        // 1. 타이틀 텍스트 항상 켜기 + 텍스트 설정
                        if (keywordObjectMap.TryGetValue("topbarTitleText", out GameObject topbarGo))
                        {
                            TMP_Text topText = topbarGo.GetComponent<TMP_Text>();
                            if (topText != null)
                                topText.text = newTitleText;

                            topbarGo.SetActive(true); // 항상 켜짐
                        }

                        // 2. 중간 텍스트 오브젝트 찾기 및 활성화
                        GameObject midTextGo = null;
                        keywordObjectMap.TryGetValue("midTitleText", out midTextGo);

                        if (midTextGo != null)
                        {
                            TMP_Text midText = midTextGo.GetComponent<TMP_Text>();
                            if (midText != null)
                                midText.text = newTitleText;

                            midTextGo.SetActive(true); // 중간 텍스트 켜기

                            // 3. 하위 애니메이션 트리거 오브젝트 켜기
                            Transform animChild = midTextGo.transform.Find("애니메이션이 들어가는 오브젝트");
                            if (animChild != null)
                            {
                                GameObject animObj = animChild.gameObject;
                                animObj.SetActive(false);
                                yield return null; // 한 프레임 기다렸다가
                                animObj.SetActive(true); // 활성화 → 자동 애니메이션
                            }

                            // 4. 대기
                            if (action.waitAfterPlay > 0f)
                                yield return new WaitForSeconds(action.waitAfterPlay);

                            // 5. 대기 후 중간 텍스트 + 애니메이션 트리거 비활성화
                            midTextGo.SetActive(false);
                        }
                        else
                        {
                            Debug.LogWarning("[컷씬] midTitleText 오브젝트가 keywordMap에 등록되지 않았습니다.");
                        }

                        break;
                    }
                case ActionType.ShowChoices:
                    {
                        if (action.choiceUIRootPrefab == null || action.choiceButtonPrefab == null || action.choiceTexts == null)
                        {
                            Debug.LogWarning("[컷씬] ShowChoices: 프리팹 또는 선택지 텍스트 누락.");
                            break;
                        }

                        // 1. 선택지 UI 전체 생성
                        GameObject root = Instantiate(action.choiceUIRootPrefab, uiRootTransform ?? transform);

                        // 2. BlockImage 활성화
                        Transform blockImageTransform = root.transform.Find("BlockImage");
                        if (blockImageTransform != null)
                            blockImageTransform.gameObject.SetActive(true); // ✅ 클릭 방지용 이미지 활성화

                        // 3. 버튼 그룹 찾기
                        Transform group = root.transform.Find("Group_Button");
                        if (group == null)
                        {
                            Debug.LogWarning("[컷씬] 선택지 UI 프리팹에 'Group_Button'이 없습니다.");
                            break;
                        }

                        // 4. 버튼 생성
                        foreach (var choice in action.choiceTexts)
                        {
                            GameObject btn = Instantiate(action.choiceButtonPrefab, group);
                            btn.SetActive(true);

                            TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
                            if (label != null) label.text = choice;

                            btn.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                clickReceived = true; // 선택 후 넘어가기
                            });
                        }

                        // 5. 사용자 클릭 대기
                        clickReceived = false;
                        while (!clickReceived)
                            yield return null;

                        // 6. BlockImage 비활성화
                        if (blockImageTransform != null)
                            blockImageTransform.gameObject.SetActive(false);

                        // 7. 전체 선택지 UI 제거
                        Destroy(root);
                        break;
                    }


            }

            if (action.activationTiming == ObjectActivationTiming.AfterAction &&
        IsKeywordControllableType(action.type))
            {
                SetActiveByKeyword(action.targetKeyword, action.setActive);
            }
        }

        EndCutscene();
    }

    private void SetActiveByKeyword(string keyword, bool setActive, float fadeDuration = 0.5f)
    {
        if (string.IsNullOrEmpty(keyword)) return;

        if (keywordObjectMap.TryGetValue(keyword, out GameObject target))
        {
            Image image = target.GetComponentInChildren<Image>();
            CanvasGroup cg = target.GetComponent<CanvasGroup>();

            if (cg != null)
            {
                if (setActive)
                {
                    target.SetActive(true);
                    cg.alpha = 0f;
                    cg.DOFade(1f, fadeDuration);
                }
                else
                {
                    cg.DOFade(0f, fadeDuration).OnComplete(() => target.SetActive(false));
                }
            }
            else if (image != null)
            {
                if (setActive)
                {
                    target.SetActive(true);
                    image.color = new Color(1, 1, 1, 0);
                    image.DOFade(1f, fadeDuration);
                }
                else
                {
                    image.DOFade(0f, fadeDuration).OnComplete(() => target.SetActive(false));
                }
            }
            else
            {
                target.SetActive(setActive);
            }
        }
        else
        {
            Debug.LogWarning($"[컷씬] 키워드 '{keyword}'에 해당하는 오브젝트가 등록되어 있지 않습니다.");
        }
    }

    private IEnumerator PlayDialogueArray(string[] texts, TMP_Text targetText)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            targetText.maxVisibleCharacters = 0;
            textTyper.StartTyping(targetText, texts[i], currentSpeed);
            yield return WaitForUserClick();
        }
    }

    private void ApplyCharacter(CutsceneAction action)
    {
        if (characterImageSlot != null)
        {
            if (action.enableCharacterFade)
            {
                characterImageSlot.DOFade(0f, 0f);
                characterImageSlot.sprite = action.characterSprite;
                characterImageSlot.gameObject.SetActive(action.characterSprite != null);
                characterImageSlot.DOFade(1f, 0.5f);
            }
            else
            {
                characterImageSlot.sprite = action.characterSprite;
                characterImageSlot.color = new Color(1f, 1f, 1f, 1f);
                characterImageSlot.gameObject.SetActive(action.characterSprite != null);
            }
        }
    }

    private IEnumerator WaitForUserClick()
    {
        clickReceived = false;
        while (true)
        {
            if (clickReceived)
            {
                if (textTyper != null && textTyper.IsTyping)
                {
                    textTyper.Skip();
                    clickReceived = false;
                }
                else
                {
                    break;
                }
            }
            yield return null;
        }
    }

    public void OnUserClick()
    {
        clickReceived = true;
    }

    private void EndCutscene()
    {
        characterDialogueText.text = "";
        narrationText.text = "";
        characterNameText.text = "";
        if (characterImageSlot != null) characterImageSlot.sprite = null;

        foreach (var slot in imageSlots)
            slot.Clear();

        characterUIRoot?.SetActive(false);
        narrationUIRoot?.SetActive(false);

        Debug.Log("[컷씬] 종료됨");
    }
    private bool IsKeywordControllableType(ActionType type)
    {
        switch (type)
        {
            case ActionType.ShowImage:
            case ActionType.CharacterSpeak:
            case ActionType.NarrationOnly:
            case ActionType.Fade:
                return true;

            case ActionType.ChangeColor: // ❌ 색상 변경은 On/Off 대상 아님
            default:
                return false;
        }
    }
}
