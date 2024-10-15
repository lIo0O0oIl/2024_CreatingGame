using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : AIState
{
    private NavMeshAgent navAgent;

    public override void OnEnterState()
    {
        navAgent = brain.navAgent;
        brain.anim.SetSpeed(0.2f);
    }

    public override void OnExitState()
    {
        brain.anim.SetSpeed(0);
    }

    public override void UpdateState()
    {
        brain.navAgent.SetDestination(aiActionData.LastSpot);
        aiActionData.is_Arrived = CheckisArrived();
        base.UpdateState();
    }

    private bool CheckisArrived()
    {
        if (navAgent.pathPending == false && navAgent.remainingDistance <= navAgent.stoppingDistance)       // 목표지점에 도달했는지
        {
            return true;
        }
        return false;
    }
}
