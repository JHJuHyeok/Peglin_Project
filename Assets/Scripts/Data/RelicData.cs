using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicData", menuName = "Item/RelicData")]
public class RelicData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    public Sprite icon;

    [Header("수치 변화")]
    public bool isStatItem = true;
    public StatType statType;
    public int value;

    [Header("특수 효과")]       // 수치를 변화시키는 효과가 아닐 때
    public string relicSpecialEffect;
}
