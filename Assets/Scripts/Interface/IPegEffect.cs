using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPegEffect
{
    void conflictEvent(GameManager game);
}

public class DullEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        // 아무 효과 없음
        Debug.Log("Dull페그 충돌");
    }
}
public class CoinEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        // 코인 획득 애니메이션

        // 코인 상승
        game.getCoin++;
        Debug.Log($"정산 예정 코인 {game.getCoin}");
    }
}
public class RefreshEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        Debug.Log("새로고침 페그 충돌");
        // 패턴 다시 불러오기
    }
}
public class CritEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        Debug.Log("크리티컬 페그 충돌");
        // 크리티컬 효과 적용
        // 일반 페그 형태 변환

    }
}
public class BombEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        Debug.Log("폭탄 페그 충돌");
        // 첫 충돌이면 이미지 변환

        // 큰 반발력

        // 폭탄 공격횟수 추가
    }
}
public class ShieldEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        // 뭐였더라?
    }
}