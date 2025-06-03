using System.Collections.Generic;
using UnityEngine;

public class LineConnectController : MonoBehaviour
{
    [SerializeField] private List<ConnectableObject> npcList; // Red → Green → Blue
    [SerializeField] private List<ConnectableObject> weaponList;
    [SerializeField] private List<ConnectableObject> placeList;
    [SerializeField] private LineRenderer redLine, greenLine, blueLine;
    [SerializeField] private GameObject checkAnswerButton;

    private ConnectableObject[,] connections = new ConnectableObject[3, 2]; // [color, 0=weapon, 1=place]
    private ConnectableObject selectedObject;
    private LineRenderer activeLine;

    private void Start()
    {
        checkAnswerButton.SetActive(false);
        foreach (var obj in weaponList) obj.colorType = ColorType.None;
        foreach (var obj in placeList) obj.colorType = ColorType.None;
        foreach (var obj in weaponList) obj.UpdateVisual();
        foreach (var obj in placeList) obj.UpdateVisual();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null)
            {
                ConnectableObject target = hit.GetComponent<ConnectableObject>();
                if (target != null) HandleClick(target);
            }
        }

        if (selectedObject != null && Input.GetMouseButton(0) && activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (selectedObject.objectType == ObjectType.NPC)
            {
                activeLine.positionCount = 2;
                activeLine.SetPosition(0, selectedObject.transform.position);
                activeLine.SetPosition(1, mousePos);
            }
            else if (selectedObject.objectType == ObjectType.Weapon)
            {
                int connectedColor = -1;
                for (int i = 0; i < 3; i++)
                {
                    if (connections[i, 0] == selectedObject) { connectedColor = i; break; }
                }
                if (connectedColor != -1)
                {
                    ConnectableObject npc = npcList[connectedColor];
                    activeLine.positionCount = 3;
                    activeLine.SetPosition(0, npc.transform.position);
                    activeLine.SetPosition(1, selectedObject.transform.position);
                    activeLine.SetPosition(2, mousePos);
                }
                else
                {
                    activeLine.positionCount = 2;
                    activeLine.SetPosition(0, selectedObject.transform.position);
                    activeLine.SetPosition(1, mousePos);
                }
            }
        }

        if (selectedObject != null && Input.GetMouseButtonUp(0) && activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null)
            {
                ConnectableObject target = hit.GetComponent<ConnectableObject>();
                if (target != null) TryConnect(selectedObject, target);
            }
            else
            {
                if (selectedObject.objectType == ObjectType.NPC)
                    DisconnectFromNpc(selectedObject.colorType);
                else if (selectedObject.objectType == ObjectType.Weapon)
                    DisconnectPlaceFromWeapon(selectedObject);
            }

            ResetSelection();
        }
    }

    private void HandleClick(ConnectableObject obj)
    {
        selectedObject = obj;
        activeLine = GetLineRenderer(obj.colorType);
        if (activeLine != null)
        {
            activeLine.positionCount = 2;
            activeLine.SetPosition(0, selectedObject.transform.position);
            activeLine.SetPosition(1, selectedObject.transform.position);
        }
    }

    private void TryConnect(ConnectableObject from, ConnectableObject to)
    {
        if (from.objectType == ObjectType.NPC && to.objectType == ObjectType.Weapon)
        {
            int colorIndex = (int)from.colorType;
            for (int i = 0; i < 3; i++)
            {
                if (connections[i, 0] == to) { DisconnectChain(i); break; }
            }
            if (connections[colorIndex, 0] != null) DisconnectChain(colorIndex);
            connections[colorIndex, 0] = to;
            to.colorType = from.colorType;
            to.UpdateVisual();
            Debug.Log($"[연결] {from.name} → {to.name}");
            UpdateLinePositions();
            CheckAllConnections();
            PrintConnections();
            return;
        }

        if (from.objectType == ObjectType.Weapon && to.objectType == ObjectType.Place)
        {
            int colorIndex = -1;
            for (int i = 0; i < 3; i++)
            {
                if (connections[i, 0] == from) { colorIndex = i; break; }
            }
            if (colorIndex == -1)
            {
                Debug.LogWarning($"[실패] {from.name}는 NPC에 연결되어 있지 않음 → {to.name}과 연결 불가");
                ForceLineClear(); return;
            }

            for (int i = 0; i < 3; i++)
            {
                if (connections[i, 1] == to)
                {
                    connections[i, 1].colorType = ColorType.None;
                    connections[i, 1].UpdateVisual();
                    connections[i, 1] = null;
                    break;
                }
            }

            connections[colorIndex, 1] = to;
            to.colorType = (ColorType)colorIndex;
            to.UpdateVisual();
            Debug.Log($"[연결] {from.name} → {to.name}");
            UpdateLinePositions();
            CheckAllConnections();
            PrintConnections();
            return;
        }

        Debug.LogWarning($"[차단] 유효하지 않은 연결 시도: {from.objectType} → {to.objectType}");
        ForceLineClear();
    }

    private void ForceLineClear()
    {
        if (activeLine != null)
            activeLine.positionCount = 0;

        UpdateLinePositions();
        CheckAllConnections();
        PrintConnections();
    }

    private void DisconnectFromNpc(ColorType npcColor)
    {
        int colorIndex = (int)npcColor;
        bool disconnected = false;
        if (connections[colorIndex, 0] != null)
        {
            Debug.Log($"[빈 공간 드롭] NPC {npcList[colorIndex].name} 연결 초기화");
            connections[colorIndex, 0].colorType = ColorType.None;
            connections[colorIndex, 0].UpdateVisual();
            if (connections[colorIndex, 1] != null)
            {
                connections[colorIndex, 1].colorType = ColorType.None;
                connections[colorIndex, 1].UpdateVisual();
                connections[colorIndex, 1] = null;
            }
            connections[colorIndex, 0] = null;
            disconnected = true;
        }
        UpdateLinePositions();
        CheckAllConnections();
        PrintConnections();
        if (!disconnected)
            Debug.Log($"[빈 공간 드롭] NPC {npcList[colorIndex].name}는 연결이 없어 아무 것도 해제되지 않음.");
    }

    private void DisconnectPlaceFromWeapon(ConnectableObject weapon)
    {
        for (int i = 0; i < 3; i++)
        {
            if (connections[i, 0] == weapon && connections[i, 1] != null)
            {
                Debug.Log($"[빈 공간 드롭] Weapon {weapon.name} → Place {connections[i, 1].name} 연결 해제");
                connections[i, 1].colorType = ColorType.None;
                connections[i, 1].UpdateVisual();
                connections[i, 1] = null;
                UpdateLinePositions();
                CheckAllConnections();
                PrintConnections();
                break;
            }
        }
    }

    private void DisconnectChain(int colorIndex)
    {
        if (connections[colorIndex, 0] != null)
        {
            connections[colorIndex, 0].colorType = ColorType.None;
            connections[colorIndex, 0].UpdateVisual();
            connections[colorIndex, 0] = null;
        }
        if (connections[colorIndex, 1] != null)
        {
            connections[colorIndex, 1].colorType = ColorType.None;
            connections[colorIndex, 1].UpdateVisual();
            connections[colorIndex, 1] = null;
        }
        Debug.Log($"[체인 해제] Color[{colorIndex}]의 연결 초기화");
        UpdateLinePositions();
        CheckAllConnections();
        PrintConnections();
    }

    private void ResetSelection()
    {
        selectedObject = null;
        activeLine = null;
    }

    private LineRenderer GetLineRenderer(ColorType colorType)
    {
        return colorType switch
        {
            ColorType.Red => redLine,
            ColorType.Green => greenLine,
            ColorType.Blue => blueLine,
            _ => null
        };
    }

    private void UpdateLinePositions()
    {
        UpdateLine(ColorType.Red);
        UpdateLine(ColorType.Green);
        UpdateLine(ColorType.Blue);
    }

    private void UpdateLine(ColorType colorType)
    {
        int i = (int)colorType;
        LineRenderer line = GetLineRenderer(colorType);
        ConnectableObject npc = npcList[i];
        ConnectableObject weapon = connections[i, 0];
        ConnectableObject place = connections[i, 1];

        if (weapon == null)
        {
            line.positionCount = 0;
            return;
        }

        if (place != null)
        {
            line.positionCount = 3;
            line.SetPosition(0, npc.transform.position);
            line.SetPosition(1, weapon.transform.position);
            line.SetPosition(2, place.transform.position);
        }
        else
        {
            line.positionCount = 2;
            line.SetPosition(0, npc.transform.position);
            line.SetPosition(1, weapon.transform.position);
        }
    }

    private void CheckAllConnections()
    {
        bool allConnected = true;
        for (int i = 0; i < 3; i++)
        {
            if (connections[i, 0] == null || connections[i, 1] == null)
            {
                allConnected = false;
                break;
            }
        }
        checkAnswerButton.SetActive(allConnected);
    }

    private void PrintConnections()
    {
        Debug.Log("=== 연결 상태 ===");
        for (int i = 0; i < 3; i++)
        {
            string npc = npcList[i].name;
            string weapon = connections[i, 0] != null ? connections[i, 0].name : "없음";
            string place = connections[i, 1] != null ? connections[i, 1].name : "없음";
            Debug.Log($"{npc} → {weapon} → {place}");
        }
    }

    public void CheckAnswerButton()
    {
        Debug.Log("[정답 체크] 호출");
    }
}
