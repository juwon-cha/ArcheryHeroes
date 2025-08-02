using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarManager : Singleton<HPBarManager>
{
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] private Transform hpBarParent;
    [SerializeField] private float hpBarOffset = 0.5f; // HPBar의 Y축 오프셋

    private List<HPBar> hpBars = new();

    private void Update()
    {
        UpdateHPBars();
    }


    public void CreateHPBar(ResourceController target)
    {
        HPBar hpBar = ObjectPoolingManager.Instance.Get<HPBar>(hpBarPrefab, target.transform.position);
        hpBar.Initialize(hpBarParent, target, hpBarOffset);
        hpBars.Add(hpBar);
    }

    public void UpdateHPBars()
    {
        foreach (var hpBar in hpBars)
        {
            if (hpBar == null) continue;
            hpBar.UpdatePosition();
        }
    }

}
