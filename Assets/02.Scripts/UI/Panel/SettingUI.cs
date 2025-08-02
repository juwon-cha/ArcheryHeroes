using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    private AudioSettingsPanel audioSettingsPanel;

    private void Awake()
    {
        audioSettingsPanel = GetComponentInChildren<AudioSettingsPanel>();
        audioSettingsPanel.Initialize();
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
