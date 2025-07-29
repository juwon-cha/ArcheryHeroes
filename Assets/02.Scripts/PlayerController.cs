using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerController : BaseController
{
    private Camera playerCamera;

    protected override void Start()
    {
        base.Start();
        playerCamera = Camera.main;
    }
    private void OnMove(InputValue inputValue)
    {
        moveDirection = inputValue.Get<Vector2>();
        moveDirection = moveDirection.normalized;
    }
}
