using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeManager : Singleton<CustomizeManager>
{
    [Header("커스터마이징 목록")]
    [SerializeField] private GameObject[] customObjects;

    public GameObject player;
    private Vector2 spawnPosition;

    void Start()
    {
        for(int i = 0;  i < customObjects.Length; i++)
        {
            Vector2 spawnPosition;
            if (i < 5)
                spawnPosition = new Vector2(-6 + 3 * i, 4);
            else
                spawnPosition = new Vector2(-6 + 3 * (i - 5), -1);
            ObjectPoolingManager.Instance.Get(customObjects[i], spawnPosition);
        }
        spawnPosition = new Vector2(0, -4);
        ObjectPoolingManager.Instance.Get(player, spawnPosition);
    }
}
