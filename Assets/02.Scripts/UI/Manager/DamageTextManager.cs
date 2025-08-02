using UnityEngine;
using TMPro;

public class DamageTextManager : Singleton<DamageTextManager>
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private float spawnOffsetRange = 1f;
    [SerializeField] private float maximumDamage = 20f; // 최대 데미지 값
    [SerializeField] private float minFadeDuration = 0.5f; // 텍스트 페이드 인 시간
    [SerializeField] private float maxFadeDuration = 1f; // 텍스트 페이드 아웃 시간
    [SerializeField] private float minTextSize = 0.5f; // 최소 텍스트 크기
    [SerializeField] private float maxTextSize = 1.5f; // 최대 텍스트 크기
    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color endColor = Color.red;


    public void ShowDamageText(int damage, Vector3 worldPosition)
    {
        Vector3 randomOffset = new(Random.Range(-spawnOffsetRange, spawnOffsetRange), Random.Range(-spawnOffsetRange, spawnOffsetRange));
        Vector2 textPos = worldPosition + randomOffset;

        float clampedDamage = Mathf.Clamp(damage, 0, maximumDamage); // 데미지 값을 최대값으로 제한
        float t = clampedDamage / maximumDamage; // 데미지 비율 계산 (0에서 1 사이)
        float fadeDuration = Mathf.Lerp(minFadeDuration, maxFadeDuration, t); // 데미지에 따라 페이드 시간 조정
        float textSize = Mathf.Lerp(minTextSize, maxTextSize, t); // 데미지에 따라 텍스트 크기 조정
        Color textColor = Color.Lerp(startColor, endColor, t); // 데미지에 따라 색상 조정

        DamageText damageText = ObjectPoolingManager.Instance.Get<DamageText>(damageTextPrefab, textPos);
        damageText.Initialize(canvasTransform, textPos, damage, textSize, fadeDuration, textColor);

    }
}
