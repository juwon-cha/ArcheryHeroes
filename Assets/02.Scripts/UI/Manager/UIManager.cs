using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIType
{
    Start,
    Setting,
    Main,
    Play,
    LevelUp,
    GameOver,
    GameClear
}

public class UIManager : Singleton<UIManager>
{
    private StartUI startUI;
    private SettingUI settingUI;
    private MainUI mainUI;
    private PlayUI playUI;
    private LevelUpUI levelUpUI;
    private GameOverUI gameOverUI;
    private GameClearUI gameClearUI;
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

        uiDictionary = new Dictionary<UIType, GameObject>
        {
            { UIType.Start, startUI.gameObject },
            { UIType.Setting, settingUI.gameObject },
            { UIType.Main, mainUI.gameObject },
            { UIType.Play, playUI.gameObject },
            { UIType.LevelUp, levelUpUI.gameObject },
            { UIType.GameOver, gameOverUI.gameObject },
            { UIType.GameClear, gameClearUI.gameObject },
        };
        sceneUIMapping = new Dictionary<string, UIType>
        {
            { "StartScene", UIType.Start },
            { "MainScene", UIType.Main },
            { "PlayScene", UIType.Play }
        };

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
}
