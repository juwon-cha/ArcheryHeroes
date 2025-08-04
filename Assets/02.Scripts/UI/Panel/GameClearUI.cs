using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] private SoundDataSO gameClearSFX; // 게임클리어 효과음
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button statisticButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(OnRestart);
        statisticButton.onClick.AddListener(OnStatistic);
        closeButton.onClick.AddListener(OnClose);
    }
    
    private void OnEnable()
    {
        AudioManager.Instance.PlaySFX(gameClearSFX);
        GameManager.Instance.Pause();
        SetStageText(DungeonManager.Instance.CurrentStageIndex);
        SetTimeText(GameManager.Instance.PlayTime);
    }

    private void OnDisable()
    {
        GameManager.Instance.Resume();
    }

    public void SetStageText(int stage)
    {
        stageText.text = $"현재 스테이지 : {stage-1}";
    }

    public void SetTimeText(float time)
    {
        timeText.text = $"진행시간 : {time:F2}";
    }

    public void OnRestart()
    {
        GameManager.Instance.ResetGame();
        FadeManager.LoadScene("PlayScene");
        UIManager.Instance.HideUI(UIType.GameOver);
    }

    public void OnStatistic()
    {

    }

    public void OnClose()
    {
        FadeManager.LoadScene("MainScene");
        UIManager.Instance.HideUI(UIType.GameOver);
    }
}
