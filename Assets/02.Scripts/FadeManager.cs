using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Image fadeImage; // 페이드 효과를 적용할 이미지
    private Coroutine fadeCoroutine;


    protected override void Initialize()
    {
        FadeOut();
        SceneManager.sceneLoaded += (_, __) => FadeOut(); // 씬이 변경될 때마다 페이드 아웃
    }

    // 씬 이동
    public static void LoadScene(string sceneName)
    {
        Instance.FadeIn(() => SceneManager.LoadScene(sceneName));
    }

    public void FadeIn(Action onComplete = null)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        AudioManager.Instance.FadeOutBGM(fadeDuration); // BGM 페이드 아웃
        fadeCoroutine = StartCoroutine(FadeCoroutine(fadeImage, 0f, 1f, onComplete));
    }

    public void FadeOut(Action onComplete = null)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        AudioManager.Instance.FadeInBGM(fadeDuration); // BGM 페이드 인
        fadeCoroutine = StartCoroutine(FadeCoroutine(fadeImage, 1f, 0f, onComplete));
    }

    IEnumerator FadeCoroutine(Image image, float from, float to, Action onComplete = null)
    {
        float time = 0f;
        Color color = image.color;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(from, to, time / fadeDuration);
            image.color = color;
            yield return null;
        }

        color.a = to;
        image.color = color;
        fadeCoroutine = null;

        onComplete?.Invoke();
    }

}
