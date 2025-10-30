using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicData", menuName = "Item/RelicData")]
public class RelicData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string itemName;
    public Sprite icon;

    [Header("��ġ ��ȭ")]
    public bool isStatItem = true;
    public StatType statType;
    public int value;

    [Header("Ư�� ȿ��")]       // ��ġ�� ��ȭ��Ű�� ȿ���� �ƴ� ��
    public string relicSpecialEffect;
}
