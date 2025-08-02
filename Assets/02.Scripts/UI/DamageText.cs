using UnityEngine;
using TMPro;
using System.Collections;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float moveSpeed = 1f;

    public void Initialize(Transform parent, Vector2 pos, int damage, float size, float fadeDuration, Color color)
    {
        transform.SetParent(parent, false); // 부모 설정
        transform.position = pos; // 초기 위치 설정
        text.text = damage.ToString();
        text.fontSize = size; // 텍스트 크기 설정
        text.color = color; // 초기 색상 설정
        StartCoroutine(FadeAndReturn(fadeDuration));
    }

    private void Update()
    {
        Move();
    }

    void Move() => transform.position += moveSpeed * Time.deltaTime * Vector3.up;

    private IEnumerator FadeAndReturn(float fadeDuration)
    {
        // 텍스트의 알파 값을 0으로 서서히 줄여나가기
        Color color = text.color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 0f;
        text.color = color;
        ObjectPoolingManager.Instance.Return(gameObject);
    }
}
