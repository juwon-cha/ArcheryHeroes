using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Action<float, float> OnExperienceChanged; // 경험치 획득 이벤트

    // TEMP Player
    [SerializeField] private GameObject playerPrefab;
    public GameObject Player { get; private set; }

    private float currentExp;
    [SerializeField] private float maxExp = 100;

    protected override void Initialize()
    {
        Player = playerPrefab;

        // 임시 플레이어 인스턴스 생성
        if (playerPrefab != null)
        {
            Player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Player.name = "Player";
        }
        else
        {
            Debug.LogError("GameManager에 Player Prefab이 지정되지 않았습니다!");
        }
    }

    public void GainExp(float exp)
    {
        currentExp += exp;
        OnExperienceChanged?.Invoke(currentExp, maxExp);
    }

    public void AddExperienceChangedEvent(Action<float, float> action)
    {
        OnExperienceChanged += action;
    }

    public void RemoveExperienceChangedEventEvent(Action<float, float> action)
    {
        OnExperienceChanged -= action;
    }

    public void EndOfWave()
    {

    }
}
