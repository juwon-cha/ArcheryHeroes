using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeSelector : MonoBehaviour
{
    public int visualIndex;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseEnter()
    {
        if(animator != null)
        {
            animator.SetBool("isMove", true);
        }
    }

    private void OnMouseExit()
    {
        if(animator != null)
        {
            animator.SetBool("isMove", false);
        }
    }

    private void OnMouseDown()
    {
        PlayerVisualManager.Instance.ChangeVisual(visualIndex);
    }
}
