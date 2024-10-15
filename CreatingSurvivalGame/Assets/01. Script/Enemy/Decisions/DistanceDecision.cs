using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DistanceDecision : AIDecision          // 거리로 조건 체크
{
    public float distance = 5f;

    public override bool CheckDecision()
    {
        if (brain.playerTrm == null) return false;

        float distance =  Vector3.Distance(brain.playerTrm.position, transform.position);

        if (distance < this.distance)
        {
            aiActionData.LastSpot = brain.playerTrm.position;
            aiActionData.playerInner = true;
        }
        else aiActionData.playerInner = false;

        return aiActionData.playerInner;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distance);
        Gizmos.color = Color.white;
    }
#endif
}
