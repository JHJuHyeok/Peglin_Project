using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public FloorData currentFloor;
    public MapNode currentNode;

    public void EnterNode(MapNode node)
    {
        currentNode = node;
        node.OnNodeEnter();
    }
}
