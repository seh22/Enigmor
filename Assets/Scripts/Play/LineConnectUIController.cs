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
        Debug.Log($"[Check ��ư] ����: {(allConnected ? "Ȱ��ȭ" : "��Ȱ��ȭ")}");
    }

    private void DrawLine(int colorIndex, Vector3[] positions)
    {
        Debug.Log($"[DrawLine] ���� {colorIndex}, �� ����: {positions.Length}");

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
        Debug.Log($"[���� ����] ���� �ε���: {colorIndex}");
    }

    private void OnDragStarted(ConnectableObject obj)
    {
        Debug.Log($"[�巡�� ����] {obj.name}");
    }

    private void OnDragEnded(ConnectableObject obj)
    {
        Debug.Log($"[�巡�� ����] {obj?.name ?? "����"}");
    }
}
