using UnityEngine;

public interface ISkill
{
    void OnAcquire(GameObject player); // 스킬을 획득했을 때 실행
    void OnRemove(GameObject player);  // 필요 시 제거
}
