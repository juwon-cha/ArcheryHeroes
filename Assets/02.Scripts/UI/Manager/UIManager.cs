using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIType
{
    Start,
    Setting,
    Main,
    Play,
    LevelUp,
    GameOver,
    GameClear,
    Achievement
}

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private SoundDataSO buttonClickSFX;

    private StartUI startUI;
    private SettingUI settingUI;
    private MainUI mainUI;
    private PlayUI playUI;
    private LevelUpUI levelUpUI;
    private GameOverUI gameOverUI;
    private GameClearUI gameClearUI;
    private AchievementListUI achievementListUI;
    private Dictionary<UIType, GameObject> uiDictionary;
    private Dictionary<string, UIType> sceneUIMapping;

    protected override void Initialize()
    {
        base.Initialize();
        startUI = GetComponentInChildren<StartUI>(true);
        settingUI = GetComponentInChildren<SettingUI>(true);
        mainUI = GetComponentInChildren<MainUI>(true);
        playUI = GetComponentInChildren<PlayUI>(true);
        levelUpUI = GetComponentInChildren<LevelUpUI>(true);
        gameOverUI = GetComponentInChildren<GameOverUI>(true);
        gameClearUI = GetComponentInChildren<GameClearUI>(true);
        achievementListUI = GetComponentInChildren<AchievementListUI>(true);

        uiDictionary = new Dictionary<UIType, GameObject>
        {
            { UIType.Start, startUI.gameObject },
            { UIType.Setting, settingUI.gameObject },
            { UIType.Main, mainUI.gameObject },
            { UIType.Play, playUI.gameObject },
            { UIType.LevelUp, levelUpUI.gameObject },
            { UIType.GameOver, gameOverUI.gameObject },
            { UIType.GameClear, gameClearUI.gameObject },
            { UIType.Achievement, achievementListUI.gameObject }
        };
        sceneUIMapping = new Dictionary<string, UIType>
        {
            { "StartScene", UIType.Start },
            { "MainScene", UIType.Main },
            { "PlayScene", UIType.Play }
        };


        playUI.Initialize();
        levelUpUI.Initialize();

        InjectButtonSFX();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HideAllUI();

        if (sceneUIMapping.TryGetValue(scene.name, out var uiType))
        {
            ShowUI(uiType);
        }
    }

    public void ShowUI(UIType uiType)
    {
        if (uiDictionary.TryGetValue(uiType, out GameObject uiObject))
            uiObject.SetActive(true);
    }
    public void ShowOnly(UIType type)
    {
        HideAllUI();
        ShowUI(type);
    }

    public void HideUI(UIType uiType)
    {
        if (uiDictionary.TryGetValue(uiType, out GameObject uiObject))
            uiObject.SetActive(false);
    }

    public void HideAllUI()
    {
        foreach (var ui in uiDictionary.Values)
            ui.SetActive(false);
    }

    // 모든 버튼에 효과음을 주입하는 메서드
    private void InjectButtonSFX()
    {
        var allButtons = FindObjectsOfType<Button>(true);
        foreach (var button in allButtons)
            button.onClick.AddListener(() => AudioManager.Instance.PlaySFX(buttonClickSFX));
    }
}
