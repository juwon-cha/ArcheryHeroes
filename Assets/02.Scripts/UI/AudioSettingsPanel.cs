using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettingsPanel : MonoBehaviour
{
    [SerializeField] private Transform sliderParent;
    [SerializeField] private VolumeSlider volumeSlider;
    private List<VolumeSlider> volumeSliders = new();

    public void Initialize()
    {
        foreach (VolumeType volumeType in Enum.GetValues(typeof(VolumeType)))
        {
            VolumeSlider slider = Instantiate(volumeSlider, sliderParent);
            slider.SetVolumeType(volumeType);
            slider.SyncSliderWithVolume();
            volumeSliders.Add(slider);
        }
    }

    public void UpdateSliders()
    {
        foreach (VolumeSlider slider in volumeSliders)
            slider.SyncSliderWithVolume();
    }

}
