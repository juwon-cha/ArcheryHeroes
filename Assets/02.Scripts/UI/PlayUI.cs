using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private UIFillBar expBar;

    public void SetStageText(string content)
    {
        if (stageText == null) return;

        stageText.text = content;
    }

    public void SetExpBar(float expPercentage)
    {
        if (expBar == null) return;

        expBar.SetFillAmount(expPercentage);
    }
}
