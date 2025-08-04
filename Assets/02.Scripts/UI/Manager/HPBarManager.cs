using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HPBarManager : Singleton<HPBarManager>
{
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] private Transform hpBarParent;
    [SerializeField] private float hpBarOffset = 0.5f; // HPBar의 Y축 오프셋

    private Dictionary<GameObject, HPBar> hpBarDict = new();

    protected override void Initialize()
    {
        hpBarDict.Clear();
    }


    private void Update()
    {
        UpdateHPBars();
    }


    public void CreateHPBar(ResourceController target)
    {
        HPBar hpBar = ObjectPoolingManager.Instance.Get<HPBar>(hpBarPrefab, target.transform.position);
        hpBar.Initialize(hpBarParent, target, hpBarOffset);
        hpBarDict[target.gameObject] = hpBar;
        // hpBars.Add(hpBar);
    }

    public void RemoveHPBar(ResourceController target)
    {
        if (hpBarDict.TryGetValue(target.gameObject, out HPBar hpBar))
        {
            ObjectPoolingManager.Instance.Return(hpBar.gameObject);
            hpBarDict.Remove(target.gameObject);
        }
    }

    public void UpdateHPBars()
    {
        foreach (var hpBar in hpBarDict.Values)
        {
            if (hpBar == null) continue;
            hpBar.UpdatePosition();
        }
    }

}
