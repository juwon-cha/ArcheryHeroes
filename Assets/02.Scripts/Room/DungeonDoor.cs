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
        doorCollider = GetComponentInChildren<BoxCollider2D>();
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

    // 문의 상태를 초기화하는 메서드
    public void ResetDoorState()
    {
        isDoorOpen = false;
        if (doorSprite != null)
            doorSprite.sprite = closedDoor;
        if (doorCollider != null)
            doorCollider.isTrigger = false;
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
        Debug.Log("플레이어가 문 트리거에 닿음");
        if(isDoorOpen && collision.CompareTag("Player"))
        {
            Debug.Log("플레이어가 다음 스테이지로 이동합니다.");
            // 던전 매니저에게 다음 방을 생성해 달라고 요청
            DungeonManager.Instance.LoadNextRoom();
        }
    }
}
