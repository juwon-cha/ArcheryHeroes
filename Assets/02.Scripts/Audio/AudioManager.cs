using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum SoundType
{
    BGM,
    SFX
}

public enum VolumeType
{
    Master,
    BGM,
    SFX
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;
    private readonly string masterVolumeParam = "Master";
    private readonly string bgmVolumeParam = "BGM";
    private readonly string sfxVolumeParam = "SFX";

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    private SoundDataSO currentBGM;
    private Coroutine fadeCoroutine;

    [Header("Sound Data List")]
    [SerializeField] private SoundDataSO[] soundDataList;
    private Dictionary<string, SoundDataSO> soundDataDict = new();
    private Dictionary<string, string> sceneBGMMapping;

    protected override void Initialize()
    {
        base.Initialize();

        foreach (var data in soundDataList)
        {
            if (!soundDataDict.ContainsKey(data.soundName))
                soundDataDict.Add(data.soundName, data);
        }

        // 씬별 BGM 매핑 초기화
        sceneBGMMapping = new Dictionary<string, string>
        {
            { "StartScene", "StartBGM" },
            { "MainScene", "MainBGM" },
            { "PlayScene", "PlayBGM" }
        };

        // 씬 변경 시 자동으로 BGM 재생
        SceneManager.sceneLoaded += (scene, _) => PlaySceneBGM(scene.name);
    }

    void PlaySceneBGM(string sceneName)
    {
        if (sceneBGMMapping != null && sceneBGMMapping.TryGetValue(sceneName, out string bgmName))
        {
            PlayBGM(bgmName);
        }
    }


    // 버튼 클릭 이벤트에서 사운드 호출용
    public static void OnPlaySound(string soundName)
    {
        Instance.PlaySound(soundName);
    }

    public static void OnPlaySound(SoundDataSO soundData)
    {
        Instance.PlaySound(soundData);
    }

    public void PlaySound(string soundName)
    {
        if (soundDataDict.TryGetValue(soundName, out SoundDataSO soundData))
            PlaySound(soundData);
    }

    public void PlaySound(SoundDataSO soundData)
    {
        if (soundData == null) return;

        if (soundData.soundType == SoundType.BGM)
            PlayBGM(soundData);
        else if (soundData.soundType == SoundType.SFX)
            PlaySFX(soundData);
    }


    // 배경음악 재생용
    public void PlayBGM(string soundName, bool loop = true)
    {
        if (soundDataDict.TryGetValue(soundName, out SoundDataSO data))
            PlayBGM(data, loop);
    }

    public void PlayBGM(SoundDataSO data, bool loop = true)
    {
        if (data == null || data.soundType != SoundType.BGM) return;

        AudioClip clip = GetRandomClip(data);
        bgmSource.loop = loop;

        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.volume = data.volume;
        bgmSource.Play();
        currentBGM = data;
    }

    // 사운드 효과 재생용
    public void PlaySFX(string soundName)
    {
        if (soundDataDict.TryGetValue(soundName, out SoundDataSO data))
            PlaySFX(data);
    }

    public void PlaySFX(SoundDataSO data)
    {
        if (data == null || data.soundType != SoundType.SFX) return;
        AudioClip clip = GetRandomClip(data);
        sfxSource.volume = data.volume;
        sfxSource.PlayOneShot(clip);
    }

    public static void SetMute(SoundType type, bool isOn)
    {
        if (type == SoundType.BGM)
            SetBGMMute(isOn);
        else if (type == SoundType.SFX)
            SetSFXMute(isOn);
    }

    public static void SetBGMMute(bool isOn)
    {
        if (Instance.bgmSource == null) return;
        Instance.bgmSource.mute = isOn;
    }

    public static void SetSFXMute(bool isOn)
    {
        if (Instance.sfxSource == null) return;
        Instance.sfxSource.mute = isOn;
    }

    private AudioClip GetRandomClip(SoundDataSO data)
    {
        if (data.audioClips == null || data.audioClips.Length == 0) return null;
        return data.audioClips[Random.Range(0, data.audioClips.Length)];
    }

    // 볼륨 조절용
    public void SetVolume(VolumeType type, float volume)
    {
        string param = type == VolumeType.Master ? masterVolumeParam :
                       type == VolumeType.BGM ? bgmVolumeParam : sfxVolumeParam;

        float volumeDB = (volume <= 0.0001f) ? -80f : Mathf.Log10(volume) * 20f;

        mixer.SetFloat(param, volumeDB);
    }

    public float GetVolume(VolumeType type)
    {
        string param = type == VolumeType.Master ? masterVolumeParam :
                       type == VolumeType.BGM ? bgmVolumeParam : sfxVolumeParam;
        mixer.GetFloat(param, out float value);
        return Mathf.Pow(10f, value / 20f);
    }
    
    // 설정 초기화용
    public void ResetVolumes()
    {
        SetVolume(VolumeType.Master, 1f);
        SetVolume(VolumeType.BGM, 1f);
        SetVolume(VolumeType.SFX, 1f);
    }
    

    // 배경음악 페이드인/아웃용
    public void FadeOutBGM(float duration)
    {
        if (currentBGM == null) return;
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeVolumeCoroutine(currentBGM.volume, 0f, duration));
    }

    public void FadeInBGM(float duration)
    {
        if (currentBGM == null) return;
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeVolumeCoroutine(0f, currentBGM.volume, duration));
    }

    private IEnumerator FadeVolumeCoroutine(float from, float to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            float volume = Mathf.Lerp(from, to, t);
            bgmSource.volume = volume;
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        bgmSource.volume = to;
    }
}
