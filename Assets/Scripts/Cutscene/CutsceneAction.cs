using System;
using UnityEngine;

public enum ActionType
{
    BackgroundChange,
    CharacterSpeak,
    ShowImage,
    NarrationOnly,
    Fade,
    Wait,
    WaitForClick,
    ChangeColor,
    SpawnPrefab,
    DespawnPrefab,
    SetTitleWithAnimation,
    ShowChoices
}

public enum FadeMode
{
    FadeIn,
    FadeOut
}

public enum ObjectActivationTiming
{
    BeforeAction,
    AfterAction
}

[Serializable]
public class CutsceneAction
{
    public string actionName;
    public ActionType type;
    public float delayBefore = 0f;

    [Header("오브젝트 On/Off (키워드 기반)")]
    public string targetKeyword; 
    public bool setActive = true;
    public ObjectActivationTiming activationTiming = ObjectActivationTiming.BeforeAction;

    [Header("배경 변경 (BackgroundChange)")]
    public Sprite backgroundSprite;

    [Header("캐릭터 대사 출력 (CharacterSpeak)")]
    public Sprite characterSprite;
    public string characterName;
    [TextArea(2, 5)] public string[] dialogueTexts;
    public bool enableCharacterFade = true;

    [Header("이미지 출력 (ShowImage)")]
    [Range(0, 9)] public int imageSlotIndex = 0;
    public Sprite imageSprite;
    public Vector2 imageSize;
    public Vector2 imagePosition;

    [Header("나레이션 출력 (NarrationOnly)")]
    [TextArea(2, 5)] public string[] narrationTexts;

    [Header("페이드/대기 (Fade, Wait)")]
    public FadeMode fadeMode = FadeMode.FadeIn;
    public float duration = 1f;

    [Header("이미지 색상 변경 (ChangeColor)")]
    public string colorTargetKeyword;
    public Color targetColor = Color.white;
    public bool useFade = true;
    public float colorChangeDuration = 0.5f;

    [Header("프리팹 생성 (SpawnPrefab)")]
    public GameObject prefabToSpawn;
    public string parentKeyword;           // 부모가 될 키워드
    public Vector3 localPosition = Vector3.zero;
    public Vector3 localScale = Vector3.one;
    public bool deactivateAfterSpawn = false;

    [Header("프리팹 제거 (DespawnPrefab)")]
    public string despawnTargetKeyword;
    public bool immediateDestroy = false;


    [Header("타이틀 + 애니메이션 (SetTitleWithAnimation)")]
    public string topbarText;                // 타이틀 & 중간 텍스트에 동일 적용
    public float waitAfterPlay = 1.5f;

    [Header("선택지 (ShowChoices)")]
    public string[] choiceTexts;               // 버튼마다 표시될 텍스트
    public GameObject choiceUIRootPrefab;     // 전체 UI (Group_Button 포함)
    public GameObject choiceButtonPrefab;     // 개별 버튼 프리팹


}
