using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteract
{
    bool Interact(int slotNum);
}

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interact")]
    [SerializeField] private float forwardOffset = 1.5f;
    [SerializeField] private float yOffset = 1.25f;
    [SerializeField] private float sphereRadius = 0.25f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private ParticleSystem particle;
    private Collider[] colliderArray = new Collider[1];        // 1개만 받을거임.
    private Collider[] particleSpawnCollider = new Collider[1];        // 1개만 받을거임.
    private bool is_colliderUse = false;
    private const float playerHeadY = 1.4f;

    private PlayerController playerController;
    private PlayerUseTool playerUseTool;

    [Header("UI")]
    [SerializeField] private GameObject interactUI;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerUseTool = GetComponent<PlayerUseTool>();
    }

    private void OnEnable()
    {
        playerController.interactAction += PlayerInteract;
    }

    private void OnDisable()
    {
        playerController.interactAction -= PlayerInteract;
    }

    private void Update()
    {
        Vector3 spherePos = transform.position + (transform.forward * forwardOffset);
        spherePos.y += yOffset;
        int colliderCount = Physics.OverlapSphereNonAlloc(spherePos, sphereRadius, colliderArray, interactLayer);

        if (colliderCount > 0)
        {
            is_colliderUse = true;
            interactUI.SetActive(true);
        }
        else
        {
            is_colliderUse = false;
            interactUI.SetActive(false);
        }
    }

    private void PlayerInteract()
    {
        Debug.Log("인터렉터");
        if (is_colliderUse)
        {
            if (colliderArray[0] != null)
            {
                if (colliderArray[0].TryGetComponent(out IPlayerInteract interactInterface))
                {
                    if (interactInterface.Interact(playerUseTool.currentSelectNum))
                    {
                        // 파티클 생성하기
                        // 레이 쏴서 해당 구역에 파티클 재생
                        Vector3 direction = (colliderArray[0].transform.position - transform.position).normalized;
                        Vector3 playerPos = transform.position;
                        playerPos.y += playerHeadY;
                        if (Physics.Raycast(playerPos, direction, out RaycastHit hit, 10, interactLayer))
                        {
                            Debug.Log("hihi");
                            particle.gameObject.transform.position = hit.point;
                            particle.Play();
                        }
                    }
                    return;
                }
            }
        }

        colliderArray[0] = null;
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (is_colliderUse) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        Vector3 spherePos = transform.position + (transform.forward * forwardOffset);
        spherePos.y += yOffset;
        Gizmos.DrawSphere(spherePos, sphereRadius);
    }
}
