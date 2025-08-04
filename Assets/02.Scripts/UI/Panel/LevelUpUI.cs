using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private SoundDataSO levelUpSFX; // 레벨업 효과음
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private int maxSelectCount = 3; // 최대 선택 개수
    [SerializeField] private GameObject levelUpSelectButtonPrefab; // 레벨업 선택 버튼 프리팹
    [SerializeField] private Transform selectButtonParents;
    private List<LevelUpSelectButton> levelUpSelectButtons;

    private void OnEnable()
    {
        Show();
    }

    private void OnDisable()
    {
        Hide();
    }

    public void Initialize()
    {
        GameManager.Instance.AddLevelUpEvent((_) => Show());

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
        var abilityList = AbilityManager.Instance.GetRandomAbilities(maxSelectCount);
        int count = Mathf.Min(abilityList.Count, maxSelectCount);

        // 능력 데이터 설정
        for (int i = 0; i < count; i++)
        {
            if (i < levelUpSelectButtons.Count)
            {
                levelUpSelectButtons[i].gameObject.SetActive(true);
                levelUpSelectButtons[i].SetAbilityData(abilityList[i]);
            }
        }

        // 나머지 버튼은 비활성화
        for (int i = count; i < levelUpSelectButtons.Count; i++)
            levelUpSelectButtons[i].gameObject.SetActive(false);
    }

    public void Show()
    {
        AudioManager.Instance.PlaySFX(levelUpSFX);
        GameManager.Instance.Pause(); // 게임 일시 정지
        gameObject.SetActive(true);
        SetSelectButtons(); // 활성화 시 선택 버튼 설정
    }


    public void Hide()
    {
        GameManager.Instance.Resume(); // 게임 재개
        gameObject.SetActive(false);
    }
}
