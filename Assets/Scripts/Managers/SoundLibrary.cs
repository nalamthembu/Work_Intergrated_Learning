using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound_Library", menuName = "Game/Sound/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    public Sound[] sounds;

    [SerializeField] MixerGroup[] groups;

    public MixerGroup? GetMixerGroup(SoundType type)
    {
        for(int i = 0; i < groups.Length; i++)
        {
            if (groups[i].type == type)
            {
                return groups[i];
            }
        }

        Debug.LogError("Could not find that specific type of mixer");

        return null;
    }
}

[System.Serializable]
public struct MixerGroup
{
    public string name;
    public AudioMixerGroup mixer;
    public SoundType type;
}