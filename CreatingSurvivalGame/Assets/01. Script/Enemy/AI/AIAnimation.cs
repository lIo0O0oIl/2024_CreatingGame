using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    private readonly int speedHash = Animator.StringToHash("Speed");        // 플레이어 속도 블렌드 트리 사용할 때 사용함.
    private readonly int attackHash = Animator.StringToHash("Attack");        // 트리거
    private readonly int is_AttackHash = Animator.StringToHash("Is_Attack");        // 트리거
    private readonly int deathHash = Animator.StringToHash("Death");        // 죽음

    public event Action attackAnimEndEvent = null;       // 애니 끝났을 때 이벤트
    public event Action attackHitCheckEvent = null;         // Event 키워드 붙여서 여기서만 invoke 하게 하기.

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

    public void OnAttackAnimEndEvent()        // 공격애니 끝났을 때 사용
    {
        attackAnimEndEvent?.Invoke();
    }

    public void OnAttackHitCheckEvent()      // 공격했을 때 플레이어 체크하기
    {
        attackHitCheckEvent?.Invoke();
    }
}
