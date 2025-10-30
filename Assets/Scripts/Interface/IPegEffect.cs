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
        // �ƹ� ȿ�� ����
    }
}
public class CoinEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        // ���� ȹ�� �ִϸ��̼�

        // ���� ���
    }
}
public class RefreshEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        // ���� �ٽ� �ҷ�����
    }
}
public class CritEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        // ũ��Ƽ�� ȿ�� ����

        // �Ϲ� ��� ���� ��ȯ

    }
}
public class BombEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        // ù �浹�̸� �̹��� ��ȯ

        // ū �ݹ߷�

        // ��ź ����Ƚ�� �߰�
    }
}
public class ShieldEffect : IPegEffect
{
    public void conflictEvent(GameManager game)
    {
        // ��������?
    }
}