using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/FloorData")]
public class FloorData : ScriptableObject
{
    public List<MapNodeData> nodes;
}
