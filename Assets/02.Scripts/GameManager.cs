using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Action<int> OnLevelUp; // 레벨업 이벤트
    private Action<float, float> OnExperienceChanged; // 경험치 획득 이벤트
    private int currentLevel = 0;

    // TEMP Player
    [SerializeField] private GameObject playerPrefab;
    public GameObject Player { get; private set; }

    private float currentExp;
    [SerializeField] private float maxExp = 100;

    private bool isPaused = false;

    protected override void Initialize()
    {
        currentLevel = 0;
        currentExp = 0f;
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

        Resume();
    }

    public void GainExp(float exp)
    {
        currentExp += exp;

        if(currentExp >= maxExp)
            StartCoroutine(LevelUpCoroutine());
        else
            OnExperienceChanged?.Invoke(currentExp, maxExp);
    }

    public void LevelUp()
    {
        Debug.Log($"레벨업! 현재 레벨: {currentLevel + 1}");
        maxExp = Mathf.Round(maxExp * 1.2f); // 레벨업 시 경험치 증가 (예: 20% 증가)
        OnLevelUp?.Invoke(++currentLevel);
    }

    IEnumerator LevelUpCoroutine()
    {
        while (currentExp >= maxExp)
        {
            // 레벨업 처리
            currentExp -= maxExp;
            LevelUp();
            OnExperienceChanged?.Invoke(currentExp, maxExp);
            yield return new WaitWhile(() => isPaused); // 일시 정지 상태가 아닐 때까지 대기
        }

    }

    public void Pause()
    {
        Time.timeScale = 0f; // 게임 일시 정지
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f; // 게임 재개
        isPaused = false;
    }

    public void AddLevelUpEvent(Action<int> action)
    {
        OnLevelUp += action;
    }

    public void RemoveLevelUpEvent(Action<int> action)
    {
        OnLevelUp -= action;
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
