using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEndDecision : AIDecision
{
    public override bool CheckDecision()        // �������� �ƴϸ�
    {
        return aiActionData.is_Attacking == false;
    }
}
