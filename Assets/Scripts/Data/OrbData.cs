using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbData", menuName = "Item/OrbData")]
public class OrbData : ScriptableObject
{
    [Header("기본 정보")]
    public string OrbName;
    public GameObject prefab;
    public int OrbLevel;
    public string description;

    [Header("오브 레벨 별 수치")]
    public int atk_One;
    public int crit_One;
    public int atk_Two;
    public int crit_Two;
    public int atk_Three;
    public int crit_Three;

    [Header("특수 효과")]
    public string OrbSpecialEffect;
}
