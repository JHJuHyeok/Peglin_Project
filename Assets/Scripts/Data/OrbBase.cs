using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBase : MonoBehaviour
{
    private PlayerManager player;
    private GameManager gameManager;
    public OrbData orbData;

    private DullEffect dullEffect;
    private CoinEffect coinEffect;
    private CritEffect critEffect;
    private BombEffect bombEffect;
    private RefreshEffect refEffect;

    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerManager>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        dullEffect = new DullEffect();
        coinEffect = new CoinEffect();
        critEffect = new CritEffect();
        refEffect = new RefreshEffect();
        bombEffect = new BombEffect();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 페그 종류 따라 함수 실행
        if(collision.gameObject.TryGetComponent<PegBase>(out PegBase peg))
        {
            peg = collision.gameObject.GetComponent<PegBase>();

            switch (peg.data.pegType)
            {
                case PegType.Dull:
                    dullEffect.conflictEvent(gameManager);
                    break;
                case PegType.Coin:
                    coinEffect.conflictEvent(gameManager);
                    break;
                case PegType.Crit:
                    critEffect.conflictEvent(gameManager);
                    break;
                case PegType.Refresh:
                    refEffect.conflictEvent(gameManager);
                    break;
                case PegType.Bomb:
                    bombEffect.conflictEvent(gameManager);
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
            player.Coin += gameManager.getCoin;
            Debug.Log($"현재 보유 코인 : {player.Coin}");
            gameManager.getCoin = 0;
            
            gameManager.ClearBall();
            gameManager.isMyTurn = true;
        }
    }
}
