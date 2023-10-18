using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public SoundLibrary library;

    private Dictionary<string, Sound> soundDictionary = new();

    public static SoundManager instance;

    private AudioSource frontendSource; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    private void Init()
    {
        for (int i = 0; i < library.sounds.Length; i++)
            soundDictionary.Add(library.sounds[i].name, library.sounds[i]);

        frontendSource = gameObject.AddComponent<AudioSource>();


        frontendSource.spatialBlend = 0;

        frontendSource.outputAudioMixerGroup = library.GetMixerGroup(SoundType.FRONTEND).Value.mixer;

        frontendSource.bypassEffects = true;
    }

    public Sound? GetSound(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            return sound;
        }

        Debug.LogError("Could not find sound : " + name);

        return null;
    }

    public void PlaySound(string name, float[] minMaxPitch = null, bool bypassEffects = false)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {

            switch (sound.type)
            {
                case SoundType.FRONTEND:

                    //This will be override in the next line, in case its not supposed to be true.
                    frontendSource.bypassEffects = true;

                    frontendSource.bypassEffects = bypassEffects;

                    frontendSource.reverbZoneMix = 1.1F;

                    frontendSource.clip = sound.clip;

                    frontendSource.Play();

                    break;


                case SoundType.SFX:

                    GameObject tmpSfx = new()
                    {
                        name = "tmp_sfx"
                    };

                    AudioSource source = tmpSfx.AddComponent<AudioSource>();

                    source.bypassEffects = bypassEffects;

                    source.clip = sound.clip;

                    source.spatialBlend = 1;

                    if (minMaxPitch != null)
                    {
                        source.pitch = Random.Range(minMaxPitch[0], minMaxPitch[1]);
                    }

                    source.outputAudioMixerGroup = library.GetMixerGroup(sound.type).Value.mixer;

                    Destroy(tmpSfx, sound.clip.length);

                    return;

                case SoundType.AMBIENCE:

                    GameObject tmpAmbience = new()
                    {
                        name = "tmp_amb"
                    };

                    source = tmpAmbience.AddComponent<AudioSource>();

                    source.bypassEffects = bypassEffects;

                    source.reverbZoneMix = 1.1F;

                    source.clip = sound.clip;

                    source.spatialBlend = 0;

                    source.outputAudioMixerGroup = library.GetMixerGroup(sound.type).Value.mixer;

                    source.Play();

                    Destroy(tmpAmbience, sound.clip.length);

                    break;
            }
        }
        else
        {
            Debug.LogError("Couldn't find sound " + name);
        }

    }

    public void PlaySound(string name, AudioSource source, bool loop = false)
    {
        if (source is null)
        {
            Debug.LogError("Audio Source is missing, you were trying to play: " + name);

            return;
        }

        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            print(sound.name + " is about to play");

            switch (sound.type)
            {
                case SoundType.SPEECH:
                case SoundType.AMBIENCE:

                    source.clip = sound.clip;

                    source.spatialBlend = 0;

                    source.loop = loop;

                    source.outputAudioMixerGroup = library.GetMixerGroup(sound.type).Value.mixer;

                    source.Play();

                    return;
            }
        }
        else
        {
            Debug.LogError("Couldn't find sound '" + name + "'");
        }
    }

    /// <summary>
    /// HOW TO USE PlaySound("name of sound from library", position in "3d space", optional min max)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="position"></param>
    /// <param name="minMaxPitch"></param>
    public void PlaySound(string name, Vector3 position, float[] minMaxPitch = null, float volume = 1)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {

            switch (sound.type)
            {
                case SoundType.SFX:

                    GameObject tmpSfx = new()
                    {
                        name = "tmp_sfx"
                    };

                    tmpSfx.transform.position = position;

                    AudioSource source = tmpSfx.AddComponent<AudioSource>();

                    source.clip = sound.clip;

                    source.spatialBlend = 1;

                    source.volume = volume;

                    if (minMaxPitch != null)
                    {
                        source.pitch = Random.Range(minMaxPitch[0], minMaxPitch[1]);
                    }

                    source.outputAudioMixerGroup = library.GetMixerGroup(sound.type).Value.mixer;

                    Destroy(tmpSfx, sound.clip.length);

                    return;

                case SoundType.AMBIENCE:

                    GameObject tmpAmbience = new()
                    {
                        name = "tmp_amb"
                    };

                    tmpAmbience.transform.position = position;

                    source = tmpAmbience.AddComponent<AudioSource>();

                    source.clip = sound.clip;

                    source.spatialBlend = 1;

                    source.volume = volume;

                    source.outputAudioMixerGroup = library.GetMixerGroup(sound.type).Value.mixer;

                    source.Play();

                    Destroy(tmpAmbience, sound.clip.length);

                    break;
            }
        }
        else
        {
            Debug.LogError("Couldn't find sound " + name);
        }

    }
}