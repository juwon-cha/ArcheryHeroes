using UnityEngine;

public interface ISkill
{
    void OnAcquire(GameObject player); // ��ų�� ȹ������ �� ����
    void OnRemove(GameObject player);  // �ʿ� �� ����
}
