using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private UIFillBar expBar;

    private void Awake()
    {
        GameManager.Instance.AddExperienceChangedEvent(SetExpBar);
    }


    public void SetStageText(string content)
    {
        if (stageText == null) return;

        stageText.text = content;
    }

    public void SetExpBar(float currentExp, float maxExp)
    {
        if (expBar == null) return;

        expBar.SetFillAmount(currentExp, maxExp);
    }
}
