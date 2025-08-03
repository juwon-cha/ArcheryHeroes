using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : UIFillBar
{
    [SerializeField] private float offset = 1f;
    private ResourceController target;
    public ResourceController Target { get; private set; }


    private Vector3 initOffset;

    public void Initialize(Transform parent, ResourceController target, float offset = 0.5f)
    {
        this.target = target;
        transform.SetParent(parent, false);
        float height = target.GetComponent<Collider2D>().bounds.size.y; // 높이 계산
        initOffset = new Vector3(0, height + offset, 0);
        UpdatePosition();
        target.AddHealthChangeEvent(OnChangeHealth);
        SetFillAmount(target.MaxHealth, target.MaxHealth);
    }

    public void UpdatePosition()
    {
        if (target == null) return;
        transform.position = target.transform.position + initOffset;
    }

    private void OnChangeHealth(float currentHP, float maxHP)
    {
        if (target == null) return;

        SetFillAmount(currentHP, maxHP);

        if (currentHP <= 0)
        {
            target.RemoveHealthChangeEvent(OnChangeHealth);
            ObjectPoolingManager.Instance.Return(gameObject);
        }
    }

}
