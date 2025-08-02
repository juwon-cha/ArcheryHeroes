using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Search;
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
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (!isMove)
            DisplayMove();

        if (isMove && !isSpawn)
            DisplaySpawnMontser();

        if (isSpawn && !isMonster)
            DisplayMonster();

        DistanceCheck();

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
        Debug.Log("몬실");
        toMoster.SetActive(true);
        if (isClose)
        {
            Debug.Log("몬삭");
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
        Collider2D obj = Physics2D.OverlapCircle(player.transform.position, 10);
        if (obj != null && obj.CompareTag("Monster"))
            isClose = true;
        else
            isClose = false;
    }
}
