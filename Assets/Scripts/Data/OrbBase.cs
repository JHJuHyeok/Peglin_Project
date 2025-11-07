using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBase : MonoBehaviour
{
    private PlayerManager player;
    private BattleManager battle;
    public OrbData orbData;

    private int orbLevel;

    private DullEffect dullEffect;
    private CoinEffect coinEffect;
    private CritEffect critEffect;
    private BombEffect bombEffect;
    private RefreshEffect refEffect;

    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerManager>();
        if (battle == null)
            battle = FindObjectOfType<BattleManager>();

        orbLevel = orbData.OrbLevel;

        dullEffect = new DullEffect();
        coinEffect = new CoinEffect();
        critEffect = new CritEffect();
        refEffect = new RefreshEffect();
        bombEffect = new BombEffect();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Peg")) return;

        if (!battle.isCritical)
            battle.damageCount += battle.normalAtk;
        else
            battle.damageCount += battle.critAtk;


        // 페그 종류 따라 함수 실행
        if (collision.gameObject.TryGetComponent<PegBase>(out PegBase peg))
        {
            peg = collision.gameObject.GetComponent<PegBase>();

            switch (peg.data.pegType)
            {
                case PegType.Dull:
                    dullEffect.conflictEvent(battle);
                    break;
                case PegType.Coin:
                        coinEffect.conflictEvent(battle);
                    break;
                case PegType.Crit:
                        critEffect.conflictEvent(battle);
                    break;
                case PegType.Refresh:
                        refEffect.conflictEvent(battle);
                    break;
                case PegType.Bomb:
                    if(peg.count == 0)
                        bombEffect.conflictEvent(battle);
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 바닥으로 떨어지면 실행
        if(collision.CompareTag("EndZone"))
        {
            // 얻은 코인 정산
            player.Coin += battle.getCoin;
            battle.CoinText.text = $"{player.Coin}";
            battle.getCoin = 0;
            
            battle.ClearBall();
            battle.isOrbFall = true;
            battle.isCritical = false;
            battle.isMyTurn = false;
            battle.isFire = false;
        }
    }
}
