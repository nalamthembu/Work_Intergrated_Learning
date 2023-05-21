using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "WheelEffectsScriptable", menuName = "Game/Vehicle/Wheel Effects")]
public class WheelEffectsScriptable : ScriptableObject
{
    public WheelSound[] wheelSounds;
    //SFX
    public AudioMixerGroup mixerGroup;
}

[System.Serializable]
public struct WheelSound
{
    public string name;
    public AudioClip startClip;
    public AudioClip loopClip;
    public AudioClip endClip;
}
