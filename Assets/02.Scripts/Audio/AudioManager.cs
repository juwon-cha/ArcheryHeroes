using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    protected override void Initialize()
    {
        base.Initialize();

        foreach (var data in soundDataList)
        {
            if (!soundDataDict.ContainsKey(data.soundName))
                soundDataDict.Add(data.soundName, data);
        }
    }

    // ��ư Ŭ�� �̺�Ʈ���� ���� ȣ���
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


    // ������� �����
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

    // ���� ȿ�� �����
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

    private AudioClip GetRandomClip(SoundDataSO data)
    {
        if (data.audioClips == null || data.audioClips.Length == 0) return null;
        return data.audioClips[Random.Range(0, data.audioClips.Length)];
    }

    // ���� ������
    public void SetVolume(VolumeType type, float volume01)
    {
        string param = type == VolumeType.Master ? masterVolumeParam :
                       type == VolumeType.BGM ? bgmVolumeParam : sfxVolumeParam;
        float volumeDB = Mathf.Log10(volume01) * 20f; // Convert to decibels
        mixer.SetFloat(param, volumeDB);
    }

    public float GetVolume(VolumeType type)
    {
        string param = type == VolumeType.Master ? masterVolumeParam :
                       type == VolumeType.BGM ? bgmVolumeParam : sfxVolumeParam;
        mixer.GetFloat(param, out float value);
        return Mathf.Pow(10f, value / 20f);
    }

    // ������� ���̵���/�ƿ���
    public void FadeOutBGM(float duration)
    {
        if (currentBGM == null) return;
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeVolumeCoroutine(bgmVolumeParam, currentBGM.volume, 0f, duration));
    }

    public void FadeInBGM(float duration)
    {
        if (currentBGM == null) return;
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeVolumeCoroutine(bgmVolumeParam, 0f, currentBGM.volume, duration));
    }

    private IEnumerator FadeVolumeCoroutine(string param, float from, float to, float duration)
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
