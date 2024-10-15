using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    protected List<AITransition> transitions;      // �ٲ�� ���µ� AITransition�ȿ� Dicision �� ����.
    protected AIBrain brain;
    protected AIActionData aiActionData;

    public abstract void OnEnterState();
    public abstract void OnExitState();

    public void SetUp(Transform root)
    {
        brain = root.GetComponent<AIBrain>();
        aiActionData = root.Find("AI").GetComponent<AIActionData>();

        transitions = new List<AITransition>();
        transform.GetComponentsInChildren<AITransition>(transitions);       // �ٷ� ����Ʈ�� �־���.

        transitions.ForEach(t => t.SetUp(root));
    }

    public virtual void UpdateState()
    {
        foreach (var transition in transitions)
        {
            if (transition.CheckTransition())
            {
                brain.ChangeState(transition.nextState);        // ������ ������ ������ȯ
                return;
            }
        }
    }
}
