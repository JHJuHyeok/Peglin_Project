using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrbEffect
{
    void ConflictEffect(BattleManager game);
}


// 이 밑으로 오브들 효과 추가
public class PebBall : OrbBase, IOrbEffect
{
    public void ConflictEffect(BattleManager game)
    {

    }
}