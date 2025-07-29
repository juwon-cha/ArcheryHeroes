using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerController : PlayerBaseController
{
    private Camera playerCamera;

    private float minDistance;

    protected override void Start()
    {
        base.Start();
        playerCamera = Camera.main;
    }

    protected override void HandleAction()
    {
        isAttacking = moveDirection == Vector2.zero ? true : false;
    }
    private void OnMove(InputValue inputValue)
    {
        moveDirection = inputValue.Get<Vector2>();
        moveDirection = moveDirection.normalized;
    }

    protected override void AttackCheck()
    {
        Collider2D[] adjObj = Physics2D.OverlapCircleAll(transform.position, weaponHandler.AttackRange);
        minDistance = weaponHandler.AttackRange;
        for (int i = 0; i < adjObj.Length; i++)
        {
            float distance;
            if (adjObj[i] != null && adjObj[i].CompareTag("Monster"))
            {
                distance = Vector2.Distance(transform.position, adjObj[i].transform.position);
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    closestTarget = adjObj[i].gameObject;
                }
            }
        }
    }
}
