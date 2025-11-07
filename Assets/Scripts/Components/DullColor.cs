using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DullColor : MonoBehaviour
{
    private void Update()
    {
        BattleManager battle = FindObjectOfType<BattleManager>();

        if (battle.isCritical)
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        else
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
