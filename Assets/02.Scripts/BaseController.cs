using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D rigid;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 moveDirection = Vector2.zero;
    public Vector2 MoveDirecition { get => moveDirection; }

    protected Vector2 lookDirection;
    public Vector2 LookDirtection { get => lookDirection; }

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        lookDirection = moveDirection;
        Rotate(lookDirection);
    }
    protected virtual void FixedUpdate()
    {
        Movement(moveDirection);
    }

    private void Movement(Vector2 direction)
    {
        direction = direction * 5;
        rigid.velocity = direction;
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90 ? true : false;

        spriteRenderer.flipX = isLeft;

        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }
    }
}
