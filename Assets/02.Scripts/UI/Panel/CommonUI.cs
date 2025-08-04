using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonUI : MonoBehaviour
{
    [SerializeField] private Button settingButton;

    private void Awake()
    {
        if (settingButton != null)
            settingButton.onClick.AddListener(OnSettingButton);
    }

    private void OnSettingButton()
    {
        UIManager.Instance.ShowUI(UIType.Setting);
    }
}
