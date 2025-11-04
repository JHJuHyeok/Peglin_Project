using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegBase : MonoBehaviour
{
    public PegData data;
    public int count;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Orb")) return;
        //Debug.Log("충돌 발생");
        count = data.breakCount;

        count--;

        if (count == 0)
            StartCoroutine(OrbConflict());
    }

    private IEnumerator OrbConflict()
    {
        yield return new WaitForSeconds(0.1f);

        this.gameObject.SetActive(false);
    }

    // 게임을 시작하거나 새로고침 함수가 실행될 때
    public void SetPeg()
    {
        count = data.breakCount;
        this.gameObject.SetActive(true);
    }
}
