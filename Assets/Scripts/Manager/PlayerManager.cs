using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatType { MaxHP, currentHP, Coin, normalAtk, critAtk, bombAtk}

public class PlayerManager : MonoBehaviour
{
    [Header("플레이어 정보")]
    public int MaxHP;           // 최대 체력
    public int currentHP;       // 현재 체력
    public int Coin;            // 현재 소지금

    [Header("전투 정보")]
    public int normalAtk;       // 보통 공격력
    public int critAtk;         // 크리티컬 공격력
    public int bombAtk = 50;    // 폭탄 공격력

    public OrbData[] myOrbList;  // 지니고 있는 오브 리스트


    public virtual void GetOrbData(OrbData data)
    {
        if (data.OrbLevel == 1)
        {
            normalAtk = data.atk_One;
            critAtk = data.crit_One;
        }
        else if(data.OrbLevel == 2)
        {
            normalAtk = data.atk_Two;
            critAtk = data.crit_Two;
        }
        else if(data.OrbLevel == 3)
        {
            normalAtk = data.atk_Three;
            critAtk = data.crit_Three;
        }
    }

    public void ModifyStat(StatType statType, int value)
    {
        switch(statType)
        {
            case StatType.MaxHP:
                MaxHP += value;
                break;
            case StatType.currentHP:
                currentHP += value;
                break;
            case StatType.Coin:
                Coin += value;
                break;
            case StatType.normalAtk:
                normalAtk += value;
                break;
            case StatType.critAtk:
                critAtk += value;
                break;
            case StatType.bombAtk:
                bombAtk += value;
                break;
        }
    }

    // 피격 시 데미지 처리
    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        // 사망한 뒤 여러가지
    }
}
