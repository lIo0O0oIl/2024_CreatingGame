using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEndDecision : AIDecision
{
    public override bool CheckDecision()        // 어택중이 아니면
    {
        return aiActionData.is_Attacking == false;
    }
}
