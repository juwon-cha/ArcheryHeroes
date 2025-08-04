using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    private AudioSettingsPanel audioSettingsPanel;
    [SerializeField] private GameObject exitButton;

    private void Awake()
    {
        audioSettingsPanel = GetComponentInChildren<AudioSettingsPanel>();
        audioSettingsPanel.Initialize();
        exitButton.SetActive(false);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if(scene.name == "TutorialScene" || scene.name == "PlayScene")
        {
            exitButton.SetActive(true);
        }
        else
        {
            exitButton.SetActive(false);
        }
    }


    public void OnExit()
    {
        FadeManager.LoadScene("MainScene");
        UIManager.Instance.HideUI(UIType.Setting);
    }

    public void OnResetSettings()
    {
        AudioManager.Instance.ResetVolumes();
        audioSettingsPanel.UpdateSliders();
    }

    public void OnClose()
    {
        UIManager.Instance.HideUI(UIType.Setting);
    }

}
