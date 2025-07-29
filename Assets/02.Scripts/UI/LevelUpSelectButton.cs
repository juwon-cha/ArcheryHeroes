using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpSelectButton : MonoBehaviour
{
    private LevelUpUI levelUpUI;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private Image skillIconIamge;
    [SerializeField] private Button selectButton;
    private SkillData skillData;

    public void Initialize(LevelUpUI _levelUpUI)
    {
        levelUpUI = _levelUpUI;

        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnButtonClick);
        }

        SetSkillData(null);
    }

    public void SetSkillData(SkillData _skillData)
    {
        skillData = _skillData;

        if (skillNameText != null)
            skillNameText.text = skillData?.SkillName;

        if (skillIconIamge != null && skillData?.Icon != null)
            skillIconIamge.sprite = skillData.Icon;
    }

    public void OnButtonClick()
    {
        if (skillData != null)
        {
            SkillManager.Instance.LevelUpSkill(skillData.skillSO);
            levelUpUI.Hide();
        }
    }

}
