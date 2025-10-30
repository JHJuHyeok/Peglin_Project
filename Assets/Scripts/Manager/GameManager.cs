using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerManager player;
    [SerializeField] private OrbData Orb;
    private Transform respawnPos;
    private Rigidbody2D orbRb;

    // ≈œ »Æ¿Œ
    private bool isMyTurn = true;

    private void Start()
    {
        respawnPos = GameObject.FindWithTag("RespawnPos").transform;
        OrbData myBall = Instantiate(Orb, respawnPos);
        orbRb = Orb.prefab.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(isMyTurn)
        {
            orbRb.gravityScale = 0.0f;
        }
    }
}
