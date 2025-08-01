using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    [Header("문 스프라이트")]
    [SerializeField] private Sprite openDoor;
    [SerializeField] private Sprite closedDoor;

    [Header("문 오브젝트")]
    [SerializeField] private SpriteRenderer doorSprite;

    private BoxCollider2D doorCollider; // 현재 문의 Collider
    [SerializeField] private bool isDoorOpen = false; // 문 열림, 닫힘 상태 변수

    private void Awake()
    {
        doorCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        CloseDoor();
    }

    public void OpenDoor()
    {
        if (isDoorOpen) return;
        isDoorOpen = true;
        doorSprite.sprite = openDoor;
        doorCollider.isTrigger = true;
    }

    public void CloseDoor()
    {
        if(!isDoorOpen) return;
        isDoorOpen = false;
        doorSprite.sprite = closedDoor;
        doorCollider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 문이 열려있는 상태이고, 플레이어일 때
        if(isDoorOpen && collision.CompareTag("Player"))
        {
            // 던전 매니저에게 다음 방을 생성해 달라고 요청
            //DungeonManager.Instance.LoadNextRoom();
        }
    }
}
