using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField] private Button customizeExitButton;

    private void Awake()
    {
        customizeExitButton.onClick.AddListener(OnExitCustomize);
    }

    public void OnExitCustomize()
    {
        ObjectPoolingManager.Instance.Return(CustomizeManager.Instance.player);
        FadeManager.LoadScene("MainScene");
    }
}
