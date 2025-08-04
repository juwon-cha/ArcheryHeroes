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
    [SerializeField] protected Sprite onImage;
    [SerializeField] protected Sprite offImage;
    [SerializeField] protected bool isOn = false;

    private void Awake()
    {
        Initialize();
    }

    public virtual void Initialize()
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
