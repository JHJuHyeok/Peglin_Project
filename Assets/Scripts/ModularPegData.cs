using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayArea/PegData")]
public class ModularPegData : ScriptableObject
{
    public string pegID;
    public string pegName;
    public GameObject prefab; 

    public PegEffectType pegEffect;
}

public enum PegEffectType
{
    Dull,
    Coin,
    Refresh,
    Crit,
    Bomb,
    Shield
}