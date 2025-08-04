using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsToggle : MonoBehaviour
{
    private void OnSetting(InputValue inputValue)
    {
        UIManager.Instance.ToggleUI(UIType.Setting);
    }
}
