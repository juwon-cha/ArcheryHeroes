using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Effect/OrbitingOrbEffect")]
public class OrbitingOrbEffect : EffectSO
{
    public GameObject orbPrefab;
    public int orbCount = 5;
    public float orbitRadius = 1f;
    public float orbitSpeed = 1f;
    
    private bool hasSpawnedOrbs = false;
    private List<GameObject> spawnedOrbs = new();

    public override void Initialize()
    {
        hasSpawnedOrbs = false;
        spawnedOrbs.Clear();
    }

    public override void Execute(EffectContext effectContext = null)
    {
        if (!hasSpawnedOrbs)
        {
            hasSpawnedOrbs = true;
            SpawnOrbs();
        }
        else
        {
            OrbitOrbs();
        }

    }

    public override void Deactivate()
    {
        foreach (var orb in spawnedOrbs)
            ObjectPoolingManager.Instance.Return(orb);

        spawnedOrbs.Clear();
    }

    void SpawnOrbs()
    {
        spawnedOrbs.Clear();

        for (int i = 0; i < orbCount; i++)
        {
            GameObject orb = ObjectPoolingManager.Instance.Get(orbPrefab, Vector3.zero);
            orb.transform.SetParent(GameManager.Instance.Player.transform);
            spawnedOrbs.Add(orb);
        }
        OrbitOrbs();
    }

    void OrbitOrbs()
    {
        for (int i = 0; i < spawnedOrbs.Count; i++)
        {
            float angle = (i / (float)orbCount) * Mathf.PI * 2f + Time.time * orbitSpeed;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * orbitRadius;
            spawnedOrbs[i].transform.localPosition = offset;
        }
    }

}
