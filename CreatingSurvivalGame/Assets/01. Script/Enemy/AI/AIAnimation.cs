using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    private readonly int speedHash = Animator.StringToHash("Speed");        // �÷��̾� �ӵ� ���� Ʈ�� ����� �� �����.
    private readonly int attackHash = Animator.StringToHash("Attack");        // Ʈ����
    private readonly int is_AttackHash = Animator.StringToHash("Is_Attack");        // Ʈ����
    private readonly int deathHash = Animator.StringToHash("Death");        // ����

    public event Action attackAnimEndEvent = null;       // �ִ� ������ �� �̺�Ʈ
    public event Action attackHitCheckEvent = null;         // Event Ű���� �ٿ��� ���⼭�� invoke �ϰ� �ϱ�.

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSpeed(float value)
    {
        animator.SetFloat(speedHash, value);
    }

    public void SetAttackTrigger(bool value)
    {
        if (value) animator.SetTrigger(attackHash);
        else animator.ResetTrigger(attackHash);
    }

    public void SetIsAttack(bool value)
    {
        animator.SetBool(is_AttackHash, value);
    }

    public void SetDeath(bool value)
    {
        animator.SetBool(deathHash, value);
    }

    public void OnAttackAnimEndEvent()        // ���ݾִ� ������ �� ���
    {
        attackAnimEndEvent?.Invoke();
    }

    public void OnAttackHitCheckEvent()      // �������� �� �÷��̾� üũ�ϱ�
    {
        attackHitCheckEvent?.Invoke();
    }
}
