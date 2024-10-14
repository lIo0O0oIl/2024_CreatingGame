using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private PlayerAnimation animator;
    private CharacterController characterController;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float gravityMultiplier = 3.0f;

    private Vector2 inputDir;
    private Vector3 moveVelocity;
    private float verticalVelocity = -1f;

    private float targetRotation = 0.0f;
    private float rotationVelocity;

    [Header("Ability")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float sprintSpeed = 5.5f;
    [SerializeField] private float cameraSpeed = 1;
    float rotationSmoothTime = 0.12f;
    private float currentPlayerSpeed = 0;
    public float jumpAmount = 2.5f;

    [Header("Actions")]
    public Action interactAction;
    public Action<int> selectSlotAction;

    [Header("GroundCheck")]
    [SerializeField] private float groundedOffset = -0.14f;
    [SerializeField] private float groundedRadius = 0.28f;
    [SerializeField] private LayerMask groundLayers;
    private bool is_Ground = false;

    [Header("Animation")]
    [SerializeField] private float freeFallTimeOut = 0.28f;
    private float currentFreeFallTime = 0.0f;

    [Header("Cinemachine")]
    [SerializeField] private GameObject cinemachineCamera;
    [SerializeField] private float cameraTopClamp = 70.0f;
    [SerializeField] private float cameraBottomClamp = -30.0f;
    private float cinemachineX;
    private float cinemachineY;

    private void Awake()
    {
        animator = GetComponent<PlayerAnimation>();
        characterController = GetComponent<CharacterController>();
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerInputs.Player.Enable();
        playerInputs.Player.Jump.performed += PlayerJump;
        playerInputs.Player.Interact.performed += PlayerInteract;
        playerInputs.Player.SelectSlot.performed += PlayerSelectSlot;
    }

    private void OnDisable()
    {
        playerInputs.Player.Jump.performed -= PlayerJump;
        playerInputs.Player.Interact.performed -= PlayerInteract;
        playerInputs.Player.SelectSlot.performed -= PlayerSelectSlot;
    }


    private void FixedUpdate()
    {
        CheckGround();
        ApplyGravity();
    }

    private void Update()
    {
        ApplyMovement();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CheckGround()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        is_Ground = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

        if (animator)
        {
            animator.SetGrounded(is_Ground);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (is_Ground) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
    }
    
    private void ApplyMovement()
    {
        inputDir = playerInputs.Player.Movement.ReadValue<Vector2>();
        float playerSpeed = playerInputs.Player.Sprint.IsPressed() ? sprintSpeed : moveSpeed;           // �ӵ� ����

        Vector3 inputDirection = new Vector3(inputDir.x, 0.0f, inputDir.y).normalized;      // ���� ��.
        if (inputDir != Vector2.zero)       // ȸ���� ����.
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);          // rotationVelocity �� ���������� ��� ��.

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            playerSpeed = 0.0f;     // �ӵ� ����.
        }

        Vector3 moveDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        moveVelocity = moveDirection * (playerSpeed * Time.deltaTime);
        moveVelocity.y = verticalVelocity * Time.deltaTime;
        characterController.Move(moveVelocity);     // ������ ����

        currentPlayerSpeed = Mathf.Lerp(currentPlayerSpeed, playerSpeed, Time.deltaTime * 10);      // �ִϸ��̼� ����
        if (currentPlayerSpeed < 0.01f) currentPlayerSpeed = 0f;
        animator.SetSpeed(currentPlayerSpeed);
    }

    private void ApplyGravity()
    {
        if (is_Ground && verticalVelocity < 0)      // �ٴڿ� ���� ��
        {
            if (animator)
            {
                animator.SetJump(false);
                animator.SetFreeFall(false);
            }

            verticalVelocity = -1f;
            currentFreeFallTime = 0f;
        }
        else
        {
            animator.SetGrounded(false);
            verticalVelocity += gravity * gravityMultiplier * Time.fixedDeltaTime;

            if (currentFreeFallTime <= freeFallTimeOut)
            {
                currentFreeFallTime += Time.fixedDeltaTime;
            }
            else
            {
                animator.SetFreeFall(true);
            }
        }
        moveVelocity.y = verticalVelocity;
    }

    private void PlayerJump(InputAction.CallbackContext context)
    {
        if (is_Ground == false) return;
        verticalVelocity = jumpAmount;
        animator.SetJump(true);
    }

    private void PlayerInteract(InputAction.CallbackContext context)
    {
        interactAction?.Invoke();
    }

    private void PlayerSelectSlot(InputAction.CallbackContext context)
    {
        string pressedKey = context.control.name;

        switch (pressedKey)
        {
            case "1":
                selectSlotAction?.Invoke(1);
                break;
            case "2":
                selectSlotAction?.Invoke(2);
                break;
            case "3":
                selectSlotAction?.Invoke(3);
                break;
            default:
                Debug.LogError("�� �Է¹����ž�");
                break;
        }
    }

    private void CameraRotation()
    {
        Vector2 mouseDelta = playerInputs.Player.Look.ReadValue<Vector2>();
        if (mouseDelta.sqrMagnitude >= 0.01)
        {
            cinemachineX += mouseDelta.y * cameraSpeed;
            cinemachineY += mouseDelta.x * cameraSpeed;
        }

        cinemachineY = ClampAngle(cinemachineY, float.MinValue, float.MaxValue);
        cinemachineX = ClampAngle(cinemachineX, cameraBottomClamp, cameraTopClamp);     // X ���� ������.

        cinemachineCamera.transform.rotation = Quaternion.Euler(cinemachineX, cinemachineY, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;            // �̰� ���� ���ص� ������ ����.
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void InputActiveSetting(bool is_Active)
    {
        if (is_Active) playerInputs.Player.Enable();
        else playerInputs.Player.Disable();
    }
}
