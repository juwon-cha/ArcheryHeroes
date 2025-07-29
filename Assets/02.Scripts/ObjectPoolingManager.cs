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

    // ������Ʈ�� �������� �޼���
    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (prefab == null) return null;

        // Dictionary�� prefab�� ������ ���� ����
        if (!pools.TryGetValue(prefab, out var pool))
        {
            pool = new();
            pools[prefab] = pool;
        }

        // Ǯ���� ������Ʈ�� �������ų� ���� ����
        var obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
        instanceToPrefab.TryAdd(obj, prefab); // �ν��Ͻ��� ������ ����

        // ��ġ�� ȸ�� ����
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }

    // ������Ʈ�� ��ȯ�ϴ� �޼���
    public void Return(GameObject obj)
    {
        // �ν��Ͻ��� �����տ� ���εǾ� �ִ��� Ȯ��
        if (!instanceToPrefab.TryGetValue(obj, out var prefab))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        pools[prefab].Enqueue(obj);
    }

    // ��� Ǯ�� ���� �޼���
    public void ClearAllPools()
    {
        foreach (var pool in pools.Values)
            while (pool.Count > 0)
                Destroy(pool.Dequeue());

        pools.Clear();
        instanceToPrefab.Clear();
    }
}
