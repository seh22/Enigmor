using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    [Header("이 스테이지에 포함된 에피소드들 (1~15)")]
    public EpisodeData[] episodes = new EpisodeData[15];
}
