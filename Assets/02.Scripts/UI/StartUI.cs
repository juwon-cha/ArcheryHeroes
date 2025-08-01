using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStart);
        settingButton.onClick.AddListener(OnSetting);
        exitButton.onClick.AddListener(OnExit);
    }

    public void OnStart()
    {
        FadeManager.LoadScene(sceneName);
    }

    public void OnSetting()
    {
        UIManager.Instance.ShowUI(UIType.Setting);
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // 에디터에서 실행 중지
#else
        Application.Quit(); // 빌드된 애플리케이션 종료
#endif
    }
}
