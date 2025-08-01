using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int isMoving = Animator.StringToHash("IsMove");
    private static readonly int isDamage = Animator.StringToHash("IsDamage");

    // 정진규 protected 에서 public 파라미터로 수정
    public Animator animator { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if(animator == null)
        {
            Debug.LogError("Animator component not found in children of " + gameObject.name);
        }
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(isMoving, obj.magnitude > 0.5f);
    }

    public void Damage()
    {
        animator.SetBool(isDamage, true);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(isDamage, false);
    }
}
