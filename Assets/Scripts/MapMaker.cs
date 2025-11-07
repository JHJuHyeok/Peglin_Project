using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [Header("맵 설정")]
    public int layerCount = 12;
    public int minNodesPerLayer = 2;
    public int maxNodesPerLayer = 4;

    private List<MapNodeData> nodes;

    [Header("맵 아이콘")]
    [SerializeField] private Sprite battleIcon;
    [SerializeField] private Sprite eliteIcon;
    [SerializeField] private Sprite treasureIcon;
    [SerializeField] private Sprite eventIcon;
    [SerializeField] private Sprite bossIcon;

    private void Start()
    {
        //nodes = generator.GenerateMap();
        DrawMap();
    }

    private void DrawMap()
    {
        Dictionary<MapNodeData, GameObject> nodeObjects = new();

        foreach(var node in nodes)
        {
            //var obj = Instantiate(nodePrefab, node.position, Quaternion.identity, transform);
            //obj.name = node.nodeType.ToString();
            //var sr = obj.GetComponent<SpriteRenderer>();
            //if (sr)
            //    sr.sprite = GetIcon(node.nodeType);
        }
    }

    // 아이콘 변경
    private Sprite GetIcon(MapNodeType type)
    {
        return type switch
        {
            MapNodeType.Battle => battleIcon,
            MapNodeType.Event => eventIcon,
            MapNodeType.Treasure => treasureIcon,
            MapNodeType.Elite => eliteIcon,
            MapNodeType.Boss => bossIcon
        };
    }
    
}
