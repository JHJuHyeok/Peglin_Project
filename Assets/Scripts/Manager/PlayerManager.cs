using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatType { MaxHP, currentHP, Coin, normalAtk, critAtk, bombAtk}

public class PlayerManager : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    public int MaxHP;           // �ִ� ü��
    public int currentHP;       // ���� ü��
    public int Coin;            // ���� ������

    [Header("���� ����")]
    public int normalAtk;       // ���� ���ݷ�
    public int critAtk;         // ũ��Ƽ�� ���ݷ�
    public int bombAtk = 50;    // ��ź ���ݷ�

    public OrbData[] myOrbList;  // ���ϰ� �ִ� ���� ����Ʈ


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

    // �ǰ� �� ������ ó��
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
        // ����� �� ��������
    }
}
