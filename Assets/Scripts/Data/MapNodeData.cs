using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapNodeData
{
    public string id;
    public Vector2 position;
    public MapNodeType nodeType;
    public List<MapNodeData> connectedNodes = new List<MapNodeData>();
}

public enum MapNodeType
{
    Battle,     // 전투
    Elite,      // 강적
    Event,      // 무작위 이벤트
    Treasure,   // 유물
    Boss        // 보스
}
