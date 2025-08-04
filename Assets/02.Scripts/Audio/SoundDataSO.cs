using UnityEngine;

[CreateAssetMenu(menuName = "Data/SoundData")]
public class SoundDataSO : ScriptableObject
{
    public string soundName;
    public AudioClip[] audioClips;
    public float volume = 1.0f;
    public SoundType soundType = SoundType.SFX;
}
