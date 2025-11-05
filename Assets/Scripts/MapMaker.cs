using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    public MapGenerator generator;
    public GameObject nodePrefab;
    public GameObject linePrefab;

    private List<MapNodeData> nodes;

    [Header("맵 아이콘")]
    [SerializeField] private Sprite battleIcon;
    [SerializeField] private Sprite eliteIcon;
    [SerializeField] private Sprite treasureIcon;
    [SerializeField] private Sprite eventIcon;
    [SerializeField] private Sprite bossIcon;

    private void Start()
    {
        nodes = generator.GenerateMap();

    }

    private void DrawMap()
    {
        Dictionary<MapNodeData, GameObject> nodeObjects = new();

        foreach(var node in nodes)
        {
            var obj = Instantiate(nodePrefab, node.position, Quaternion.identity, transform);
            obj.name = node.nodeType.ToString();
            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr)
                sr.sprite = GetIcon(node.nodeType);
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
