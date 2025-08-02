using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public GameObject move;
    public GameObject toRight;
    public GameObject toMoster;
    public GameObject stop;
    public GameObject next;

    public PlayerController player;

    private bool isMove;
    private bool isRight;
    private bool isMonster;
    private bool isStop;
    private bool isNext;

    public bool isClose;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        DisplayMove();

        DisplayRight();

        DisplayMonster();

        DistanceCheck();
        
        DisplayStop();

        DisplayNext();
    }

    private void DisplayMove()
    {
        if (!isMove)
        {
            move.SetActive(true);
            if (player.MovementDirection != Vector2.zero)
            {
                move.SetActive(false);
                isMove = true;
            }
        }
    }

    private void DisplayRight()
    {
        if(isMove && !isRight)
        {
            Vector3 pivot = new Vector3(5, 0, 0);
            toRight.SetActive(true);
            if(player.transform.position.x > pivot.x)
            {
                toRight.SetActive(false);
                isRight = true;
            }
        }    
    }

    private void DisplayMonster()
    {
        if(isRight && !isMonster)
        {
            toMoster.SetActive(true);
            if(isClose)
            {
                toMoster.SetActive(false);
                isMonster = true;
            }
        }
    }

    private void DisplayStop()
    {
        if(isMonster && !isStop)
        {
            if (isClose)
            {
                stop.SetActive(true);
                if(player.MovementDirection == Vector2.zero)
                {
                    stop.SetActive(false);
                    isStop = true;
                }
            }
        }
    }

    private void DisplayNext()
    {
        if(isStop && !isNext && isClose)
        {
            next.SetActive(true);
        }
    }

    private void DistanceCheck()
    {
        Collider2D obj = Physics2D.OverlapCircle(player.transform.position, 5);
        if (obj != null && obj.CompareTag("Monster"))
            isClose = true;
        else
            isClose = false;
    }
}
