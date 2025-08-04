using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungronSkillBox : MonoBehaviour
{
    // 한 번 사용한 박스인지 확인
    private bool hasBeenUsed = false;
    private DungeonRoom parentRoom;

    private void Awake()
    {
        parentRoom = GetComponentInParent<DungeonRoom>();
    }

    // 플레이어가 상자에 닿았을 때 호출됩니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 이미 사용되었거나, 닿은 것이 플레이어가 아니면 아무것도 하지 않습니다.
        if (hasBeenUsed || !other.CompareTag("Player"))
        {
            return;
        }

        // 상자를 사용된 상태로 만듭니다. (중복 실행 방지)
        hasBeenUsed = true;

        //    UIManager에게, 다른 데이터 없이 그냥 'LevelUp' 타입의 UI를
        //    보여달라고만 요청합니다.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowUI(UIType.LevelUp);
        }
        else
        {
            Debug.LogError("UIManager를 찾을 수 없습니다!");
        }
        
        // 문을 열기 위해 상자에 닿았다고 알린다.
        if (parentRoom != null)
        {
            parentRoom.OnEventObjectInteracted();
        }
        // 상자 오브젝트 자체는 역할을 다했으므로 비활성화합니다.
        // 근대 비활성화 할지 빈 껍대기로 만들지는 선택
        gameObject.SetActive(false);
    }
}
