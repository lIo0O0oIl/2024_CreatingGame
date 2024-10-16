using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerUseTool playerUseTool;
    private PlayerAnimation playerAnim;
    private PlayerStat playerStat;

    [SerializeField] private float attackTime;
    private WaitForSeconds delay;

    [Header("AttackCheck")]
    [SerializeField] private float forwardOffset = 1.5f;
    [SerializeField] private float yOffset = 1.25f;
    [SerializeField] private float attackRadius = 1;
    [SerializeField] private LayerMask interactLayer;
    private Collider[] colliderArray = new Collider[1];        // 1개만 받을거임.

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerUseTool = GetComponent<PlayerUseTool>();
        playerAnim = GetComponent<PlayerAnimation>();
        playerStat = GetComponent<PlayerStat>();

        delay = new WaitForSeconds(attackTime);
    }

    private void OnEnable()
    {
        playerController.attackAction += Attack;
        playerAnim.attackCheckEvent += AttackHitCheck;
    }

    private void OnDisable()
    {
        playerController.attackAction -= Attack;
        playerAnim.attackCheckEvent -= AttackHitCheck;
    }

    private void Attack()
    {
        if (playerUseTool.currentSelectNum != 3)
        {
            Debug.Log("다른 도구 들어");
            return;
        }

        // 공격
        playerAnim.SetAttackTrigger();
        playerController.playerInputs.Player.Movement.Disable();
        playerController.playerInputs.Player.SelectSlot.Disable();
        playerController.playerInputs.Player.Attack.Disable();

        StartCoroutine(DontMove());
    }

    private IEnumerator DontMove()
    {
        yield return delay;

        playerController.playerInputs.Player.Movement.Enable();
        playerController.playerInputs.Player.SelectSlot.Enable();
        playerController.playerInputs.Player.Attack.Enable();
    }

    private void AttackHitCheck()
    {
        Vector3 spherePos = transform.position + (transform.forward * forwardOffset);
        spherePos.y += yOffset;
        Physics.OverlapSphereNonAlloc(spherePos, attackRadius, colliderArray, interactLayer);

        if (colliderArray[0] != null)
        {
            if (colliderArray[0].TryGetComponent(out IHitInterface hitInterface))
            {
                hitInterface.Damage(playerStat.myDamage);
                GameManager.Instance.StatisticsManager.OneAddStatistic(Statistics.Attack);
            }
        }

        colliderArray[0] = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 spherePos = transform.position + (transform.forward * forwardOffset);
        spherePos.y += yOffset;
        Gizmos.DrawWireSphere(spherePos, attackRadius);
        Gizmos.color = Color.white;
    }
}
