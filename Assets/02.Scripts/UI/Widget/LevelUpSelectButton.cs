using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpSelectButton : MonoBehaviour
{
    private LevelUpUI levelUpUI;
    [SerializeField] private TMP_Text abilityNameText;
    [SerializeField] private Image abilityIconIamge;
    [SerializeField] private Button selectButton;
    private AbilityData abilityData;

    public void Initialize(LevelUpUI _levelUpUI)
    {
        levelUpUI = _levelUpUI;

        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnButtonClick);
        }

        SetAbilityData(null);
    }

    public void SetAbilityData(AbilityData _skillData)
    {
        abilityData = _skillData;

        if (abilityNameText != null)
            abilityNameText.text = abilityData?.AbilityName;

        if (abilityIconIamge != null && abilityData?.Icon != null)
            abilityIconIamge.sprite = abilityData.Icon;
    }

    public void OnButtonClick()
    {
        if (abilityData != null)
        {
            AbilityManager.Instance.LevelUpSkill(abilityData.abilitySO);
            levelUpUI.Hide();
        }
    }

}
