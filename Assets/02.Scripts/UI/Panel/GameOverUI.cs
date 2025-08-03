using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
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
        GameManager.Instance.Pause();
        SetStageText(DungeonManager.Instance.CurrentStageIndex);
        SetTimeText(Time.time);
    }

    private void OnDisable()
    {
        GameManager.Instance.Resume();
    }

    public void SetStageText(int stage)
    {
        stageText.text = $"현재 스테이지 : {stage}";
    }

    public void SetTimeText(float time)
    {
        timeText.text = $"Time: {time:F2} seconds";
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
