using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbData", menuName = "Item/OrbData")]
public class OrbData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string OrbName;
    public GameObject prefab;
    public int OrbLevel;

    [Header("���� ���� �� ��ġ")]
    public int atk_One;
    public int crit_One;
    public int atk_Two;
    public int crit_Two;
    public int atk_Three;
    public int crit_Three;

    [Header("Ư�� ȿ��")]
    public string OrbSpecialEffect;
}
