using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudioPlayer : MonoBehaviour
{
    public SoundDataSO soundData;

    public void OnEnable()
    {
        AudioManager.Instance.PlaySound(soundData);
    }
}
