using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    protected List<AITransition> transitions;      // 바뀌는 상태들 AITransition안에 Dicision 이 있음.
    protected AIBrain brain;
    protected AIActionData aiActionData;

    public abstract void OnEnterState();
    public abstract void OnExitState();

    public void SetUp(Transform root)
    {
        brain = root.GetComponent<AIBrain>();
        aiActionData = root.Find("AI").GetComponent<AIActionData>();

        transitions = new List<AITransition>();
        transform.GetComponentsInChildren<AITransition>(transitions);       // 바로 리스트에 넣어줌.

        transitions.ForEach(t => t.SetUp(root));
    }

    public virtual void UpdateState()
    {
        foreach (var transition in transitions)
        {
            if (transition.CheckTransition())
            {
                brain.ChangeState(transition.nextState);        // 조건이 맞으면 상태전환
                return;
            }
        }
    }
}
