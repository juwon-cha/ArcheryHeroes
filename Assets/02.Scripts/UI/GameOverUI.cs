using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
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
