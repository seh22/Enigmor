using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Episode
{
    public int episode;
    public bool cleared;
}

[System.Serializable]
public class Stage
{
    public int stage;
    public List<Episode> episodes;
}

[System.Serializable]
public class GameData
{
    public List<Stage> stages;
}


public class EpClearDataManager : Singleton<EpClearDataManager>
{
    public GameData gameData;
    public int selectedStage; // 선택한 스테이지
    public int selectedEpisode; // 선택한 에피소드

    private string dataPath;

    public override void Awake()
    {
        base.Awake(); // Singleton Awake 호출

        dataPath = Path.Combine(Application.dataPath, "JsonData/StageData.json");

        LoadGameData();
    }


    /// <summary>
    /// 게임 데이터를 저장
    /// </summary>
    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(dataPath, json);
        Debug.Log($"Game data saved to: {dataPath}");
    }


    /// <summary>
    /// 게임 데이터를 로드
    /// </summary>
    public void LoadGameData()
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game data loaded successfully.");
        }
        else
        {
            // 파일이 없으면 기본 데이터 초기화
            InitializeDefaultData();
            SaveGameData();
        }
    }

    /// <summary>
    /// 기본 데이터를 초기화
    /// </summary>
    private void InitializeDefaultData()
    {
        gameData = new GameData
        {
            stages = new List<Stage>
            {
                new Stage { stage = 1, episodes = CreateEpisodes(15) },
                new Stage { stage = 2, episodes = CreateEpisodes(15) },
                new Stage { stage = 3, episodes = CreateEpisodes(15) },
            }
        };
    }

    /// <summary>
    /// 특정 수의 에피소드를 생성
    /// </summary>
    private List<Episode> CreateEpisodes(int count)
    {
        var episodes = new List<Episode>();
        for (int i = 1; i <= count; i++)
        {
            episodes.Add(new Episode { episode = i, cleared = false });
        }
        return episodes;
    }

    /// <summary>
    /// 특정 에피소드를 클리어 처리
    /// </summary>
    public void MarkEpisodeCleared(int stage, int episode)
    {
        var targetStage = gameData.stages.Find(s => s.stage == stage);
        if (targetStage != null)
        {
            var targetEpisode = targetStage.episodes.Find(e => e.episode == episode);
            if (targetEpisode != null)
            {
                targetEpisode.cleared = true;
                Debug.Log("Ep" + targetEpisode.cleared);
                SaveGameData(); // 클리어 상태 저장

            }
        }
    }

    /// <summary>
    /// 특정 에피소드가 클리어 되었는지 확인
    /// </summary>
    public bool IsEpisodeCleared(int stage, int episode)
    {
        var targetStage = gameData.stages.Find(s => s.stage == stage);
        return targetStage?.episodes.Find(e => e.episode == episode)?.cleared ?? false;
    }

    /// <summary>
    /// 현재 스테이지, 에피소드
    /// </summary>
    public void SetSelectedEpisode(int stage, int episode)
    {
        selectedStage = stage;
        selectedEpisode = episode;
    }

}



