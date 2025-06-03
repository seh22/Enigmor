using System;
using UnityEngine;

public static class LineConnectEvents
{
    public static Action<bool> OnAllLinesConnectedChanged;
    public static Action<int, Vector3[]> OnLineUpdated;
    public static Action<int> OnLineCleared;
    public static Action<ConnectableObject> OnDragStarted;
    public static Action<ConnectableObject> OnDragEnded;
}