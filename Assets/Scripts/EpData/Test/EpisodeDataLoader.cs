using UnityEngine;

public class EpisodeDataLoader : MonoBehaviour
{
    [Header("모든 스테이지 데이터 (1~3)")]
    [SerializeField] private StageData[] stageDatas = new StageData[3];

    /// <summary>특정 스테이지, 에피소드의 데이터를 반환합니다.</summary>
    public EpisodeData GetEpisode(int stageIndex, int episodeIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageDatas.Length)
        {
            Debug.LogError($"Invalid stageIndex: {stageIndex}");
            return null;
        }
        var stage = stageDatas[stageIndex];
        if (episodeIndex < 0 || episodeIndex >= stage.episodes.Length)
        {
            Debug.LogError($"Invalid episodeIndex: {episodeIndex}");
            return null;
        }
        return stage.episodes[episodeIndex];
    }
}
