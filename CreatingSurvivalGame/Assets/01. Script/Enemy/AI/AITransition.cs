using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransition : MonoBehaviour
{
    public AIDecision[] decisions;
    public AIState nextState;

    public void SetUp(Transform root)
    {
        foreach (var d in decisions)
        {
            d.SetUp(root);
        }
    }

    public bool CheckTransition()
    {
        bool result = false;
        foreach (var d in decisions)
        {
            result = d.CheckDecision();
            if (d.is_Reverse)
            {
                result = !result;
            }
            if (result == false) break;
        }
       
        return result;
    }
}
