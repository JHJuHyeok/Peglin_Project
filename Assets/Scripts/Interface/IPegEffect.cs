using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPegEffect
{
    void conflictEvent(BattleManager game);
}

public class DullEffect : IPegEffect
{
    public void conflictEvent(BattleManager game)
    {
        // 아무 효과 없음
    }
}
public class CoinEffect : IPegEffect
{
    public void conflictEvent(BattleManager game)
    {
        // 코인 획득 애니메이션

        // 코인 상승
        game.getCoin++;
    }
}
public class RefreshEffect : IPegEffect
{
    public void conflictEvent(BattleManager game)
    {
        // 패턴 다시 불러오기
        game.RemovePegs(game.pegAlign);
        game.CreatePegs(game.pegAlign, 2, 2, 8);
    }
}
public class CritEffect : IPegEffect
{
    public void conflictEvent(BattleManager game)
    {
        // 크리티컬
        game.isCritical = true;
    }
}
public class BombEffect : IPegEffect
{
    public void conflictEvent(BattleManager game)
    {
        // 큰 반발력

        // 폭탄 공격횟수 추가
        game.bombCount++;
    }
}
public class ShieldEffect : IPegEffect
{
    public void conflictEvent(BattleManager game)
    {
        // 뭐였더라?
    }
}