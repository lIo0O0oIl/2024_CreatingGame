using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIBrain : MonoBehaviour
{
    [SerializeField] private AIState currentState;
    [SerializeField] public Transform playerTrm;

    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public AIAnimation anim;

    private BoxCollider myColider;

    private bool is_Death = false;
    public bool Is_Death => is_Death;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<AIAnimation>();
        myColider = GetComponent<BoxCollider>();

        List<AIState> states = new List<AIState>();
        transform.Find("AI").GetComponentsInChildren<AIState>(states);

        states.ForEach(s =>  s.SetUp(transform));

        if (playerTrm == null) Debug.LogError("플레이어 넣어줘라");
        if (currentState == null) Debug.LogError("AI 현 상태 넣어줘라");
    }

    public void SetDestination(Vector3 pos)
    {
        navAgent.SetDestination(pos);       // 플레이어 쫒을 때
    }

    public void ChangeState(AIState nextState)
    {
        currentState?.OnExitState();
        currentState = nextState;
        currentState?.OnEnterState();
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void Death(bool is_ItemGive = true)
    {
        if (is_Death == false)
        {
            currentState = null;        // 널로하거나 아이들로 바꾸거나 아예 스크립트를 끄거나.
            myColider.enabled = false;
            anim.SetDeath(true);

            if (is_ItemGive == true) GameManager.Instance.GiveEnemyItem();
            is_Death = true;
        }
    }
}
