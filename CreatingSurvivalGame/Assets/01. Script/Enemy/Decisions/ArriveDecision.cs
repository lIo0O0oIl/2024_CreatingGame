using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveDecision : AIDecision        // 목표지점에 도달했는지
{
    public override bool CheckDecision()
    {
        return aiActionData.is_Arrived;
    }
}
