using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomizeManager : Singleton<CustomizeManager>
{
    [Header("커스터마이징 목록")]
    [SerializeField] private GameObject[] customObjects;

    public GameObject previewPrefab;
    public GameObject player;
    private Vector2 playerSpawnPosition;

    protected override void Initialize()
    {
        player = null;
        playerSpawnPosition = new Vector2(0, -4);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode __)
    {
        if (scene.name != "CustomizingScene") return;

        for (int i = 0; i < customObjects.Length; i++)
        {
            Vector2 spawnPosition;
            if (i < 5)
                spawnPosition = new Vector2(-6 + 3 * i, 2);
            else
                spawnPosition = new Vector2(-6 + 3 * (i - 5), -1);
            ObjectPoolingManager.Instance.Get(customObjects[i], spawnPosition);
        }

        player = ObjectPoolingManager.Instance.Get(previewPrefab, playerSpawnPosition);
    }
}
