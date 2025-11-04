using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatType { MaxHP, currentHP, Coin, Buff_Str, Buff_Crit, Buff_bomb }

public class PlayerManager : MonoBehaviour
{
    [Header("플레이어 정보")]
    public int MaxHP;           // 최대 체력
    public int currentHP;       // 현재 체력
    public int Coin;            // 현재 소지금

    [Header("전투 정보")]
    public int Buff_Str;        // 추가 보통 공격력
    public int Buff_Crit;       // 추가 크리티컬 공격력
    public int Buff_bomb;       // 추가 폭탄 공격력

    public List<OrbData> myOrbList = new List<OrbData>();         // 지니고 있는 오브 리스트
    public List<RelicData> myRelicList = new List<RelicData>();     // 지니고 있는 유물 리스트
    
    // 싱글톤 디자인
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 보장
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        MaxHP = 80;
        currentHP = MaxHP;
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
            case StatType.Buff_Str:
                Buff_Str += value;
                break;
            case StatType.Buff_Crit:
                Buff_Crit += value;
                break;
            case StatType.Buff_bomb:
                Buff_bomb += value;
                break;
        }
    }

    public void AddOrb(OrbData orb)
    {
        if(orb != null && myOrbList.Contains(orb))
        {
            myOrbList.Add(orb);
            Debug.Log($"오브 추가 : {orb.OrbName}");
        }
    }

    public void RemoveOrb(OrbData orb)
    {
        myOrbList.Remove(orb);
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
        // 사망 애니메이션 재생

        // 게임 오버 씬으로 이동
    }
}
