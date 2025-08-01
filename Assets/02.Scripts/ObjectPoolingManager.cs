using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolingManager : Singleton<ObjectPoolingManager>
{
    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();
    private readonly Dictionary<GameObject, GameObject> instanceToPrefab = new();

    protected override void Initialize()
    {
        base.Initialize();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene _, LoadSceneMode __) => ClearAllPools();

    // 오브젝트를 가져오는 메서드
    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (prefab == null) return null;

        // Dictionary에 prefab이 없으면 새로 생성
        if (!pools.TryGetValue(prefab, out var pool))
        {
            pool = new();
            pools[prefab] = pool;
        }

        // 풀에서 오브젝트를 가져오거나 새로 생성
        var obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
        instanceToPrefab.TryAdd(obj, prefab); // 인스턴스와 프리팹 매핑

        // 위치와 회전 설정
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }


    public GameObject Get(GameObject prefab, Vector3 pos)
    {
        return Get(prefab, pos, Quaternion.identity);
    }


    // 오브젝트를 반환하는 메서드
    public void Return(GameObject obj)
    {
        // 인스턴스가 프리팹에 매핑되어 있는지 확인
        if (!instanceToPrefab.TryGetValue(obj, out var prefab))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        pools[prefab].Enqueue(obj);
    }

    // 모든 풀을 비우는 메서드
    public void ClearAllPools()
    {
        foreach (var pool in pools.Values)
            while (pool.Count > 0)
                Destroy(pool.Dequeue());

        pools.Clear();
        instanceToPrefab.Clear();
    }
}
