using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveDecision : AIDecision        // ��ǥ������ �����ߴ���
{
    public override bool CheckDecision()
    {
        return aiActionData.is_Arrived;
    }
}
