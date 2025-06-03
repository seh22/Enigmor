using UnityEngine;
using UnityEngine.UI;

public class LineConnectUIController : MonoBehaviour
{
    [Header("UI")]
    public Button checkAnswerButton;
    public LineRenderer[] lineRenderers = new LineRenderer[3]; // Red, Green, Blue

    private void OnEnable()
    {
        LineConnectEvents.OnAllLinesConnectedChanged += UpdateCheckAnswerButton;
        LineConnectEvents.OnLineUpdated += DrawLine;
        LineConnectEvents.OnLineCleared += ClearLine;
        LineConnectEvents.OnDragStarted += OnDragStarted;
        LineConnectEvents.OnDragEnded += OnDragEnded;
    }

    private void OnDisable()
    {
        LineConnectEvents.OnAllLinesConnectedChanged -= UpdateCheckAnswerButton;
        LineConnectEvents.OnLineUpdated -= DrawLine;
        LineConnectEvents.OnLineCleared -= ClearLine;
        LineConnectEvents.OnDragStarted -= OnDragStarted;
        LineConnectEvents.OnDragEnded -= OnDragEnded;
    }

    public void UpdateCheckAnswerButton(bool allConnected)
    {
        checkAnswerButton.interactable = allConnected;
        Debug.Log($"[Check 버튼] 상태: {(allConnected ? "활성화" : "비활성화")}");
    }

    private void DrawLine(int colorIndex, Vector3[] positions)
    {
        Debug.Log($"[DrawLine] 색상 {colorIndex}, 점 개수: {positions.Length}");

        LineRenderer line = lineRenderers[colorIndex];
        if (positions.Length < 2)
        {
            line.positionCount = 0;
            return;
        }

        line.positionCount = positions.Length;
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 fixedPos = positions[i];
            fixedPos.z = -1f;
            line.SetPosition(i, fixedPos);
        }
    }

    private void ClearLine(int colorIndex)
    {
        lineRenderers[colorIndex].positionCount = 0;
        Debug.Log($"[라인 제거] 색상 인덱스: {colorIndex}");
    }

    private void OnDragStarted(ConnectableObject obj)
    {
        Debug.Log($"[드래그 시작] {obj.name}");
    }

    private void OnDragEnded(ConnectableObject obj)
    {
        Debug.Log($"[드래그 종료] {obj?.name ?? "없음"}");
    }
}
