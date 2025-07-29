using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHandler : WeaponHandler
{
    [Header("Melee Attack Info")]
    public Vector2 ColliderBoxSize = Vector2.one; // �ٰŸ� ����(�浹) ����

    protected override void Start()
    {
        base.Start();

        ColliderBoxSize = ColliderBoxSize * WeaponSize; // ���� ũ�⿡ ���� �浹 ���� ũ�� ����
    }

    public override void Attack()
    {
        base.Attack();

        RaycastHit2D hit = Physics2D.BoxCast(
            // ���� ��ġ���� �ٶ󺸴� �������� �ڽ� ĳ��Ʈ
            transform.position + (Vector3)EnemyController.LookDirection * ColliderBoxSize.x, // ��ġ
            ColliderBoxSize, // ������
            0f, // ����
            Vector2.zero, // ����
            0f, // �Ÿ�
            Target // ���̾� ����ũ
        );

        if (hit.collider != null)
        {
            ResourceController resourceController = hit.collider.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                resourceController.ChangeHealth(-Power); // ������ ����
                if (IsOnKnockBack)
                {
                    // �˹� ó��
                    EnemyController controller = hit.collider.GetComponent<EnemyController>();
                    if (controller != null)
                    {
                        controller.ApplyKnockBack(transform, KnockBackPower, KnockBackDuration);
                    }
                }
            }
        }
    }

    public override void Rotate(bool isLeft)
    {
        if (isLeft)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // �������� ȸ��
        }
        else
        {
            transform.eulerAngles = Vector3.zero; // ���������� ȸ��
        }
    }
}
