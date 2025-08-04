using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeSelector : MonoBehaviour
{
    public int visualIndex;
    public GameObject effect;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnMouseEnter()
    {
        if(animator != null)
        {
            animator.SetBool("IsMove", true);
            transform.localScale = new Vector3(2.5f, 2.5f, 1f);
            effect.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if(animator != null)
        {
            animator.SetBool("IsMove", false);
            transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            effect.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        PlayerVisualManager.Instance.ChangeVisual(visualIndex);
    }
}
