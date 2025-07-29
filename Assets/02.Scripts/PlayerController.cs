using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Camera playerCamera;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform weaponPivot;

    private Vector2 moveDirection = Vector2.zero;
    public Vector2 MoveDirecition { get => moveDirection; }

    private Vector2 lookDirection;
    public Vector2 LookDirtection { get => lookDirection; }

    private int moveSpeed = 5;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerCamera = Camera.main;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        lookDirection = moveDirection;
        Rotate(lookDirection);
    }

    private void FixedUpdate()
    {
        Movement(moveDirection);
    }

    private void Movement(Vector2 direction)
    {
        direction = direction * moveSpeed;
        rigid.velocity = direction;
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90 ? true : false;

        spriteRenderer.flipX = isLeft;

        if(weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }
    }

    private void OnMove(InputValue inputValue)
    {
        moveDirection = inputValue.Get<Vector2>();
        moveDirection = moveDirection.normalized;
    }
}
