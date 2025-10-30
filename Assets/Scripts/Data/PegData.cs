using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayArea/PegData")]
public class PegData : ScriptableObject
{
    public string pegID;
    public string pegName;
    public GameObject prefab; 

    public PegType pegType;
}

public enum PegType
{
    Dull,
    Coin,
    Refresh,
    Crit,
    Bomb,
    Shield
}