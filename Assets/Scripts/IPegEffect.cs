using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPegEffect
{
    void conflictEvent();
}

public class DullEffect : IPegEffect
{
    public void conflictEvent()
    {
        // �ƹ� ȿ�� ����
    }
}
public class CoinEffect : IPegEffect
{
    public void conflictEvent()
    {
        // ���� ȹ�� �ִϸ��̼�

        // ���� ���
    }
}
public class RefreshEffect : IPegEffect
{
    public void conflictEvent()
    {
        // ���� �ٽ� �ҷ�����
    }
}
public class CritEffect : IPegEffect
{
    public void conflictEvent()
    {
        // ũ��Ƽ�� ȿ�� ����

        // �Ϲ� ��� ���� ��ȯ

    }
}
public class BombEffect : IPegEffect
{
    public void conflictEvent()
    {
        // ù �浹�̸� �̹��� ��ȯ

        // ū �ݹ߷�

        // ��ź ����Ƚ�� �߰�
    }
}
public class ShieldEffect : IPegEffect
{
    public void conflictEvent()
    {
        // ��������?
    }
}