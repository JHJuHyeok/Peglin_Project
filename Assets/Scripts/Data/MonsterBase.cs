using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public MonsterData data;
    public int HP;
    public int Atk;
    public int currentPoint;
    public bool isCanAttack;

    private void Start()
    {
        HP = data.hp;
        Atk = data.atk;
    }

    public void Damaged(int damage)
    {
        if (HP > 0)
            HP -= damage;
        else
            Dead();
    }

    public void Attaked(int playerHp)
    {
        if (isCanAttack)
            playerHp -= Atk;
        else return;
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
