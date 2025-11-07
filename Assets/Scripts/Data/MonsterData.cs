using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string name;
    public string Id;
    public int hp;
    public int atk;
    public bool isCanAttack;
}
