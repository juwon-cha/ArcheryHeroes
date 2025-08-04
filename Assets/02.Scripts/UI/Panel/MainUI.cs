using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private Button customizingButton;
    [SerializeField] private Button achievementButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        customizingButton.onClick.AddListener(OnCustomizing);
        achievementButton.onClick.AddListener(OnAchievement);
        startButton.onClick.AddListener(OnStartStage);
        exitButton.onClick.AddListener(OnExitGame);
    }

    public void OnCustomizing()
    {
        FadeManager.LoadScene("CustomizingScene");
    }

    public void OnAchievement()
    {
        UIManager.Instance.ShowUI(UIType.Achievement);
    }

    public void OnStartStage()
    {
        FadeManager.LoadScene("TutorialScene");
        // FadeManager.LoadScene("PlayScene");
        //FadeManager.LoadScene("PlayScene_TestJJG"); // 임시 테스트용
    }

    public void OnExitGame()
    {
        FadeManager.LoadScene("StartScene");
    }
}
