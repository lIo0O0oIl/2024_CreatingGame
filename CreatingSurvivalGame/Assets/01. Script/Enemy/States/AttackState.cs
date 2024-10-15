using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AIState
{
    [SerializeField] private float rotateSpeed = 720f;

    private Vector3 playerTrm;

    [Header("AttackCheck")]
    [SerializeField] private int damage = 30;
    [SerializeField] private float forwardOffset = 1.5f;
    [SerializeField] private float yOffset = 1.25f;
    [SerializeField] private float attackRadius = 1;
    [SerializeField] private LayerMask interactLayer;
    private Collider[] colliderArray = new Collider[1];        // 1개만 받을거임.

    public override void OnEnterState()
    {
        brain.navAgent.SetDestination(transform.position);      // 멈추고
        brain.anim.attackAnimEndEvent += AtttackAnimEndEvent;
        brain.anim.attackHitCheckEvent += AttackHitCheck;
        aiActionData.is_Attacking = false;
    }

    public override void OnExitState()
    {
        brain.anim.attackAnimEndEvent -= AtttackAnimEndEvent;
        brain.anim.attackHitCheckEvent -= AttackHitCheck;

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

                float direction = result.y > 0 ? 1 : -1;
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

    private void AttackHitCheck()
    {
        Vector3 direction = brain.playerTrm.position - brain.transform.position;
        Debug.Log(Vector3.Dot(direction, brain.transform.forward));
        if (Vector3.Dot(direction, brain.transform.forward) > 0) return;

        Vector3 spherePos = transform.position + (brain.playerTrm.position - transform.position).normalized * forwardOffset;
        spherePos.y += yOffset;

        Physics.OverlapSphereNonAlloc(spherePos, attackRadius, colliderArray, interactLayer);

        if (colliderArray[0] != null)
        {
            if (colliderArray[0].TryGetComponent(out IHitInterface hitInterface))
            {
                Debug.Log("공격들어감");
                hitInterface.Damage(damage);
            }
        }

        colliderArray[0] = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (brain == null) return;
        Gizmos.color = Color.red;
        Vector3 spherePos = transform.position + (brain.playerTrm.position - transform.position).normalized * forwardOffset;
        spherePos.y += yOffset;
        Gizmos.DrawWireSphere(spherePos, attackRadius);
        Gizmos.color = Color.white;
    }
}
