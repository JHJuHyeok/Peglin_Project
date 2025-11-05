using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/MapGenerator")]
public class MapGenerator : ScriptableObject
{
    [Header("맵 구조 설정")]
    public int floorCount = 7;
    public int nodesCount = 3;
    public float xSpacing = 3f;
    public float ySpacing = 3f;

    [Header("각 노드 확률")]
    public float eventRand = 0.2f;
    public float treasureRand = 0.1f;
    public float eliteRand = 0.15f;

    public List<MapNodeData> GenerateMap()
    {
        List<MapNodeData> allNodes = new List<MapNodeData>();
        List<MapNodeData> previousFloor = null;
        List<MapNodeData> currentFloor = new List<MapNodeData>();

        for (int i = 0; i < floorCount; i++)
        {
            float xOffset = -(nodesCount - 1) * xSpacing / 2f;

            for (int j = 0; j < nodesCount; j++)
            {
                MapNodeData node = new MapNodeData
                {
                    position = new Vector2(xOffset + j * xSpacing, i * ySpacing),
                    nodeType = GetRandomNodeType(floorCount)
                };
                currentFloor.Add(node);
                allNodes.Add(node);
            }

            if (previousFloor != null)
            {
                foreach (var node in previousFloor)
                {
                    int count = Random.Range(1, 2);
                    for (int k = 0; k < count; k++)
                    {
                        var next = currentFloor[Random.Range(0, currentFloor.Count)];
                        if (!node.connectedNodes.Contains(next))
                            node.connectedNodes.Add(next);
                    }
                }
            }

            previousFloor = currentFloor;
        }

        // 마지막 노드는 보스 방
        foreach (var node in previousFloor)
            node.nodeType = MapNodeType.Boss;

        return allNodes;
    }



    private MapNodeType GetRandomNodeType(int floorIndex)
    {
        if (floorIndex == 0) return MapNodeType.Battle;

        float r = Random.value;
        if (r < eventRand) return MapNodeType.Event;
        if (r < eventRand + treasureRand) return MapNodeType.Treasure;
        if (r < eventRand + treasureRand + eliteRand) return MapNodeType.Elite;

        return MapNodeType.Battle;
    }
}
