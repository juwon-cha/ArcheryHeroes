using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private int maxSelectCount = 3; // 최대 선택 개수
    [SerializeField] private GameObject levelUpSelectButtonPrefab; // 레벨업 선택 버튼 프리팹
    [SerializeField] private Transform selectButtonParents;
    private List<LevelUpSelectButton> levelUpSelectButtons;
    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        SetSelectButtons();
    }

    void Initialize()
    {
        Debug.Log("LevelUpUI Initialized");

        if (titleText != null)
            titleText.text = "Level Up";

        levelUpSelectButtons = new();

        // 레벨업 선택 버튼 초기화
        for (int i = 0; i < maxSelectCount; i++)
        {
            var buttonObj = Instantiate(levelUpSelectButtonPrefab, selectButtonParents);
            var button = buttonObj.GetComponent<LevelUpSelectButton>();
            button.Initialize(this);
            levelUpSelectButtons.Add(button);
        }
    }

    void SetSelectButtons()
    {
        Debug.Log("Setting Select Buttons");

        var skillList = SkillManager.Instance.GetRandomSkills(maxSelectCount);
        int count = Mathf.Min(skillList.Count, maxSelectCount);

        for (int i = 0; i < count; i++)
        {
            if (i < levelUpSelectButtons.Count)
            {
                levelUpSelectButtons[i].SetSkillData(skillList[i]);
            }
        }

        // 나머지 버튼은 비활성화
        for (int i = count; i < levelUpSelectButtons.Count; i++)
        {
            levelUpSelectButtons[i].gameObject.SetActive(false);
        }
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
