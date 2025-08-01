using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    None,
    Player,
    Enemy
}

public class DamageTrigger : MonoBehaviour
{

    [SerializeField] private TeamType teamType = TeamType.Enemy; // 팀 타입
    [SerializeField] private float damage = 5f;    // 한 번의 틱마다 입힐 대미지 양
    private ResourceController reesourceController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어에게 데미지 적용
        if (collision.CompareTag("Player"))
        {
            if (teamType == TeamType.Player) return;

            reesourceController = collision.GetComponent<ResourceController>();

            if (reesourceController != null)
            {
                reesourceController.ChangeHealth(-damage);
            }
        }
        // 몬스터에게 데미지 적용
        else if (collision.CompareTag("Monster"))
        {
            if (teamType == TeamType.Enemy) return;

            reesourceController = collision.GetComponent<ResourceController>();

            if (reesourceController != null)
            {
                reesourceController.ChangeHealth(-damage);
            }
        }
    }

}
