using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [Tooltip("NPC/Weapon/Place 이름")]
    public string name;

    [Tooltip("NPC/Weapon/Place 이미지")]
    public Sprite image;

    [Tooltip("설명 및 단서 정보를 [0]=description, [1]=clue 순서로 담습니다.")]
    public string[] info = new string[2];

    [Tooltip("연결용 고유 ID")]
    public int objectId;
}
