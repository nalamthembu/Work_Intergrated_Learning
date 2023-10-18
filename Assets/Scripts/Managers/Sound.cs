using UnityEngine;

[System.Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
    public SoundType type;
}

public enum SoundType
{
    SFX,
    MUSIC,
    SPEECH,
    AMBIENCE,
    FRONTEND
}