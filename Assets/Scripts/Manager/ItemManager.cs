using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton
{

    public void ApplyEffect(RelicData relic, PlayerManager player, GameManager game)
    {
        // 수치를 변환할 때
        if (relic.isStatItem)
        {
            player.ModifyStat(relic.statType, relic.value);
            Debug.Log($"{relic.itemName}: {relic.statType} +{relic.value}");
        }
        // 특수 효과를 적용할 때
        else if (!string.IsNullOrEmpty(relic.relicSpecialEffect))
        {
            // 클래스 이름으로 IRelicEffect 인스턴스 생성
            Type t = Type.GetType(relic.relicSpecialEffect);

            if (t != null && Activator.CreateInstance(t) is IRelicEffect effect)
            {
                effect.ApplyEffect(game);
            }
            else
            {
                Debug.LogWarning($"특수 효과를 찾을 수 없습니다: {relic.relicSpecialEffect}");
            }
        }
    }
}
