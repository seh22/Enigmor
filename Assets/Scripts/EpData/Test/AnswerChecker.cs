public static class AnswerChecker
{
    /// <summary>
    /// 플레이어가 선택한 objectId 배열과 에피소드의 correctCode를 비교합니다.
    /// </summary>
    public static bool IsCorrect(int[] playerSelection, EpisodeData episode)
    {
        if (episode == null || playerSelection == null) return false;
        if (playerSelection.Length != episode.correctCode.Length) return false;
        for (int i = 0; i < playerSelection.Length; i++)
            if (playerSelection[i] != episode.correctCode[i])
                return false;
        return true;
    }
}
