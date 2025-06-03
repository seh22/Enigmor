using UnityEngine;

[CreateAssetMenu(fileName = "EpisodeData", menuName = "Game/Episode Data")]
public class EpisodeData : ScriptableObject
{
    [Header("에피소드 기본 정보")]
    [Tooltip("1~3 스테이지 번호")]
    public int stageNumber;
    [Tooltip("1~15 에피소드 번호")]
    public int episodeNumber;
    [Tooltip("에피소드 제목")]
    public string episodeTitle;
    [TextArea(3,10), Tooltip("인트로 내러티브")]
    public string introText;

    [Header("NPC 3명 정보")]
    public CharacterData[] npcCharacters = new CharacterData[3];

    [Header("Weapon 3개 정보")]
    public CharacterData[] weaponObjects = new CharacterData[3];

    [Header("Place 3개 정보")]
    public CharacterData[] placeObjects = new CharacterData[3];

    [Header("정답 코드 (npcId, weaponId, placeId 순)")]
    public int[] correctCode = new int[3];

    [Header("에피소드 힌트들")]
    [Tooltip("힌트 문장들을 순서대로 배열에 담습니다.")]
    public string[] hints;
}
