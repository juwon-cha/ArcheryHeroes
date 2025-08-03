using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private UIFillBar expBar;

    public void Initialize()
    {
        GameManager.Instance.AddExperienceChangedEvent(SetExpBar);
        DungeonManager.Instance.AddStageChangedEvent((dungeonIndex) => SetStageText($"현재 스테이지 {dungeonIndex}"));
    }


    public void SetStageText(string content)
    {
        Debug.Log($"SetStageText: {content}");
        if (stageText == null) return;

        stageText.text = content;
    }

    public void SetExpBar(float currentExp, float maxExp)
    {
        if (expBar == null) return;

        expBar.SetFillAmount(currentExp, maxExp);
    }
}
