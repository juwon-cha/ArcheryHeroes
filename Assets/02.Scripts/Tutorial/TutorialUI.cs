using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public GameObject move;
    public GameObject spawnMonster;
    public GameObject toMoster;
    public GameObject stop;
    public GameObject next;

    public PlayerController player;

    private bool isMove;
    private bool isSpawn;
    private bool isMonster;
    public bool isStop;
    private bool isNext;

    public bool isClose;


    private void Start()
    {
        // player = FindObjectOfType<PlayerController>();
        player = GameManager.Instance.Player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        DistanceCheck();
        if (!isMove)
            DisplayMove();

        if (isMove && !isSpawn)
            DisplaySpawnMontser();

        if (isSpawn && !isMonster)
            DisplayMonster();

        if (isMonster && !isStop)
            DisplayStop();

        if (isStop && !isNext)
            DisplayNext();
    }

    private void DisplayMove()
    {
        move.SetActive(true);
        if (player.isInterAct)
        {
            move.SetActive(false);
            isMove = true;
            player.isInterAct = false;
        }
    }

    private void DisplaySpawnMontser()
    {
        spawnMonster.SetActive(true);
        if (player.isInterAct == true)
        {
            spawnMonster.SetActive(false);
            isSpawn = true;
        }
    }

    private void DisplayMonster()
    {
        toMoster.SetActive(true);
        if (isClose)
        {
            toMoster.SetActive(false);
            isMonster = true;
        }
    }

    private void DisplayStop()
    {
        stop.SetActive(true);
        if (player.isInterAct)
        {
            stop.SetActive(false);
            isStop = true;
        }
    }

    private void DisplayNext()
    {
        next.SetActive(true);
        isNext = true;
    }

    private void DistanceCheck()
    {
        isClose = false;
        Collider2D[] objs = Physics2D.OverlapCircleAll(player.transform.position, 7);
        foreach (Collider2D obj in objs)
        {
            if (obj != null && obj.CompareTag("Monster"))
            {
                isClose = true;
                break;
            }
        }
    }
}
