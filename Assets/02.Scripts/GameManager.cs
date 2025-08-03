using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Action<int> OnLevelUp; // 레벨업 이벤트
    private Action<float, float> OnExperienceChanged; // 경험치 획득 이벤트
    private int currentLevel = 0;

    // TEMP Player
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    public GameObject Player
    {
        get
        {
            if (player == null)
            {
                player = ObjectPoolingManager.Instance.Get(playerPrefab, Vector3.zero);
                player.name = "Player";
            }

            return player;
        }
    }

    private float currentExp;
    private float maxExp;
    [SerializeField] private float initMaxExp = 10f; // 초기 최대 경험치

    private bool isPaused = false;

    protected override void Initialize()
    {
        currentLevel = 0;
        currentExp = 0f;
        maxExp = initMaxExp;
        OnExperienceChanged?.Invoke(currentExp, maxExp);
        Resume();
    }

    public void ResetGame()
    {
        SkillManager.Instance.ResetSkills();
        AbilityManager.Instance.ResetAbilities();
        Initialize();
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
