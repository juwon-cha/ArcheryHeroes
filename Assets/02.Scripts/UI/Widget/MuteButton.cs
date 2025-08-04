using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteButton : ToggleButton
{
    public SoundType soundType;

    public override void Initialize()
    {
        base.Initialize();

        OnToggleOn.AddListener(() => AudioManager.SetMute(soundType, true));
        OnToggleOff.AddListener(() => AudioManager.SetMute(soundType, false));
    }

}
