using UnityEngine;
using UnityEngine.UI;


public class UIFillBar : MonoBehaviour
{
    [SerializeField] private Image fillTarget;

    private void Awake()
    {
        if (fillTarget == null) return;

        fillTarget.type = Image.Type.Filled;
    }
    public void SetFillAmount(float current, float max)
    {
        if (fillTarget == null) return;

        float percentage = max > 0 ? current / max : 0f;
        SetFillAmount(percentage);
    }

    public void SetFillAmount(float fillPercentage)
    {
        if (fillTarget == null) return;

        fillTarget.fillAmount = Mathf.Clamp01(fillPercentage);
    }
}
