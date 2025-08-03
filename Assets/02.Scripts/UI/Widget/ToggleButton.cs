using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public UnityEvent OnToggleOn;
    public UnityEvent OnToggleOff;

    private Button toggleButton;
    [SerializeField] private Sprite onImage;
    [SerializeField] private Sprite offImage;
    [SerializeField] private bool isOn = false;

    private void Awake()
    {
        toggleButton = GetComponentInChildren<Button>(true);
        if (toggleButton != null)
            toggleButton.onClick.AddListener(OnToggle);

        SetToggleState(isOn);
    }

    public void SetToggleState(bool isOn)
    {
        if (toggleButton == null) return;
        this.isOn = isOn;
        toggleButton.image.sprite = isOn ? onImage : offImage;
    }

    public void OnToggle()
    {
        isOn = !isOn;
        SetToggleState(isOn);

        if (isOn)
            OnToggleOn?.Invoke();
        else
            OnToggleOff?.Invoke();
    }
}
