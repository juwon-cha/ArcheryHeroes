using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button statisticButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(OnRestart);
        statisticButton.onClick.AddListener(OnStatistic);
        closeButton.onClick.AddListener(OnClose);
    }



    public void OnRestart()
    {
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
