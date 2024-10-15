using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AIState
{
    [SerializeField] private float rotateSpeed = 720f;

    private Vector3 playerTrm;

    public override void OnEnterState()
    {
        brain.navAgent.SetDestination(transform.position);      // 멈추고
        brain.anim.OnAnimationEndTrigger += AtttackAnimEndEvent;
        aiActionData.is_Attacking = false;
    }

    public override void OnExitState()
    {
        brain.anim.OnAnimationEndTrigger -= AtttackAnimEndEvent;

        brain.anim.SetIsAttack(false);
        brain.anim.SetAttackTrigger(false);
    }

    public override void UpdateState()
    {
        base.UpdateState();     // 조건 확인

        if (aiActionData.is_Attacking == false)
        {
            playerTrm = brain.playerTrm.position - transform.position;
            playerTrm.y = 0;

            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(forward, playerTrm);

            if (angle >= 10f)       // 각도 차이가 너무 크면 회전
            {
                Vector3 result = Vector3.Cross(forward, playerTrm);

                float direction = result.y >  0 ? 1 : -1;
                brain.transform.rotation = Quaternion.Euler(0, direction * rotateSpeed * Time.deltaTime, 0) * brain.transform.rotation;
            }
            else
            {
                aiActionData.is_Attacking = true;
                brain.anim.SetIsAttack(true);
                brain.anim.SetAttackTrigger(true);
            }
        }
    }

    private void AtttackAnimEndEvent()
    {
        brain.anim.SetIsAttack(false);
        aiActionData.is_Attacking = false;
    }
}
