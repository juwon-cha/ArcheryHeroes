using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StartUI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";

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
