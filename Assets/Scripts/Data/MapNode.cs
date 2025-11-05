using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public MapNodeType nodeType;
    public MapNodeData nodeData;

    public void Initialize(MapNodeData data)
    {
        nodeData = data;
        nodeType = data.nodeType;
        //UpdateVisual();
    }

    // 노드 진입 시 씬 로드
    public void OnNodeEnter()
    {
        switch (nodeType)
        {
            case MapNodeType.Battle:
                break;
            case MapNodeType.Elite:
                break;
            case MapNodeType.Event:
                break;
            case MapNodeType.Treasure:
                break;
            case MapNodeType.Boss:
                break;
        }
    }
}
