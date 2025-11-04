using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DullColor : MonoBehaviour
{
    private void Update()
    {
        BattleManager battle = FindObjectOfType<BattleManager>();

        if (battle.isCritical)
            this.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
