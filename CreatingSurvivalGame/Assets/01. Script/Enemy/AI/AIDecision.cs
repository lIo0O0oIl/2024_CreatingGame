using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision : MonoBehaviour
{
    public bool is_Reverse = false;
    protected AIActionData aiActionData;
    protected AIBrain brain;

    public virtual void SetUp(Transform root)
    {
        brain = root.GetComponent<AIBrain>();
        aiActionData = root.Find("AI").GetComponent<AIActionData>();
    }

    public abstract bool CheckDecision();
}
