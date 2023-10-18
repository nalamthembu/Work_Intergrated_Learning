using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class LevelEventTrigger : MonoBehaviour
{
    new private BoxCollider2D collider;

    private EVENT_TYPE eventType;

    [SerializeField] protected bool CanOnlyBeTriggeredOnce;

    protected void Init(EVENT_TYPE eventType)
    {
        collider = GetComponent<BoxCollider2D>();

        collider.isTrigger = true;

        this.eventType = eventType;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerEvent(eventType);

            if (CanOnlyBeTriggeredOnce)
            {
                collider.enabled = false;
            }
        }
    }

    private void TriggerEvent(EVENT_TYPE eventType)
    {
        switch (eventType)
        {
            case EVENT_TYPE.PLAY_SOUND:

                LevelEventSound soundEv = (LevelEventSound)this;

                PlaySoundEvent evParams_Sound = soundEv.eventParameters;

                print("sound event triggered, attempting to play, " + evParams_Sound.soundName);


                if (evParams_Sound.isStaticSound)
                {
                    SoundManager.instance.PlaySound
                        (
                            evParams_Sound.soundName,
                            evParams_Sound.position,
                            evParams_Sound.minMaxPitch,
                            evParams_Sound.volume
                        );

                    return;
                }

                SoundManager.instance.PlaySound
                        (
                            evParams_Sound.soundName,
                            evParams_Sound.minMaxPitch,
                            evParams_Sound.bypassEffects
                        );

                break;


            case EVENT_TYPE.SPAWN_ENEMIES:

                LevelEventSpawnEnemies enemyEv = (LevelEventSpawnEnemies)this;

                SpawnEnemiesEvent evParams_Enemy = enemyEv.eventParameters;

                print("spawn enemy event triggered, attempting to spawn");

                if (evParams_Enemy.enemyPrefab is not null)
                {
                    enemyEv.InitialiseEnemies();
                }

                //TO-DO : ATMOSPHERIC SOUND PLAYED HERE?

                break;

            case EVENT_TYPE.CHANGE_AMBIENCE:

                LevelEventChangeAmbience ambEv = (LevelEventChangeAmbience)this;

                ChangeAmbienceEvent evParams_ChangeAmbience = ambEv.eventParameters;

                print("attempting, changing world ambience");

                if (AmbienceSoundManager.instance is null)
                {
                    Debug.LogError("Please make sure the Ambience Manager is in the scene.");
                    return;
                }

                AmbienceSoundManager.instance.SetCurrentAmbience(evParams_ChangeAmbience.AmbienceToChangeTo);

                break;
                
        }
    }
}

public enum EVENT_TYPE
{
    PLAY_SOUND,
    SPAWN_ENEMIES,
    CHANGE_AMBIENCE,
    TRIGGER_DIALOGUE
}

[System.Serializable]
public struct PlaySoundEvent
{
    public string soundName;

    public bool isStaticSound;

    public Vector2 position;

    public float[] minMaxPitch;

    public bool bypassEffects;

    public float volume;
}

[System.Serializable]
public struct SpawnEnemiesEvent
{
    public GameObject enemyPrefab;

    [Header("World Position")]
    public Transform[] spawnPoints;

    public EnemyWave[] waves;

    [HideInInspector]
    public bool eventComplete;
}

[System.Serializable]
public struct EnemyWave
{
    public int enemyCount;

    [HideInInspector] public bool allEnemiesEliminated;
}

[System.Serializable]
public struct ChangeAmbienceEvent
{
    public string AmbienceToChangeTo;
}


[System.Serializable]
public struct TriggerDialogueEvent
{

}