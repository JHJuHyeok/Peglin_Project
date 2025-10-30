using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public void ApplyEffect(RelicData relic, PlayerManager player, GameManager game)
    {
        // ��ġ�� ��ȯ�� ��
        if (relic.isStatItem)
        {
            player.ModifyStat(relic.statType, relic.value);
            Debug.Log($"{relic.itemName}: {relic.statType} +{relic.value}");
        }
        // Ư�� ȿ���� ������ ��
        else if (!string.IsNullOrEmpty(relic.relicSpecialEffect))
        {
            // Ŭ���� �̸����� IRelicEffect �ν��Ͻ� ����
            Type t = Type.GetType(relic.relicSpecialEffect);

            if (t != null && Activator.CreateInstance(t) is IRelicEffect effect)
            {
                effect.ApplyEffect(game);
            }
            else
            {
                Debug.LogWarning($"Ư�� ȿ���� ã�� �� �����ϴ�: {relic.relicSpecialEffect}");
            }
        }
    }
}
