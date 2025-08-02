using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class UIFillBar : MonoBehaviour
{
    [SerializeField] private Image fillTarget;
    [SerializeField] private Image delayFillTarget;
    [SerializeField] private float delayFillSpeed = 0.5f;
    [SerializeField] private bool useDelayFill = false;
    private Coroutine delayFillCoroutine;

    private void Awake()
    {
        if (fillTarget == null) return;

        fillTarget.type = Image.Type.Filled;
    }
    public void SetFillAmount(float current, float max)
    {
        if (fillTarget == null) return;

        float percentage = max > 0 ? current / max : 0f;
        fillTarget.fillAmount = Mathf.Clamp01(percentage);

        if (useDelayFill)
            SetDelayFillAmount(percentage);
    }

    public void SetDelayFillAmount(float fillPercentage)
    {
        if (gameObject.activeInHierarchy == false) return;
        if (delayFillTarget == null || !useDelayFill) return;
        if (delayFillCoroutine != null)
            StopCoroutine(delayFillCoroutine);

        delayFillCoroutine = StartCoroutine(DelayFillCoroutine(fillPercentage));
    }

    private IEnumerator DelayFillCoroutine(float targetFill)
    {
        if (delayFillTarget == null || !useDelayFill) yield break;

        float startFill = delayFillTarget.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < delayFillSpeed)
        {
            elapsedTime += Time.deltaTime;
            float newFill = Mathf.Lerp(startFill, targetFill, elapsedTime / delayFillSpeed);
            delayFillTarget.fillAmount = Mathf.Clamp01(newFill);
            yield return null;
        }

        delayFillTarget.fillAmount = targetFill; // Ensure it ends exactly at the target fill
    }
}
