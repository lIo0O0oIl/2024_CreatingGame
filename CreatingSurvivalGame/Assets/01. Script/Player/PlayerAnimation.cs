using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAnimation : MonoBehaviour
{
    private readonly int speedHash = Animator.StringToHash("Speed");        // 플레이어 속도 블렌드 트리 사용할 때 사용함.
    private readonly int jumpHash = Animator.StringToHash("Jump");
    private readonly int groundedHash = Animator.StringToHash("Grounded");
    private readonly int freeFallHash = Animator.StringToHash("FreeFall");
    private readonly int motionSpeedHash = Animator.StringToHash("MotionSpeed");
    private readonly int attackTriggerHash = Animator.StringToHash("Attack");
    private readonly int deathHash = Animator.StringToHash("Death");

    public event Action attackCheckEvent;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat(speedHash, speed);
    }

    public void SetJump(bool is_Jump)
    {
        animator.SetBool(jumpHash, is_Jump);
    }

    public void SetGrounded(bool is_Ground)
    {
        animator.SetBool(groundedHash, is_Ground);
    }

    public void SetFreeFall(bool is_FreeFall)
    {
        animator.SetBool(freeFallHash, is_FreeFall);
    }

    public void SetMotionSpeed(float motionSpeed)
    {
        animator.SetFloat(motionSpeedHash, motionSpeed);
    }

    public void SetAttackTrigger()
    {
        animator.SetTrigger(attackTriggerHash);
    }

    public void SetDeath(bool value)
    {
        animator.SetBool(deathHash, value);
    }
    
    private void OnAttackCheck()
    {
        attackCheckEvent?.Invoke();
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        Debug.Log("발소리 출력");
/*        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }*/
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        Debug.Log("착지 소리 출력");
/*        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }*/
    }
}
