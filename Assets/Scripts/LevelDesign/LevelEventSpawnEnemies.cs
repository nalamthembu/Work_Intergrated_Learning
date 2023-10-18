using UnityEngine;

public class LevelEventSpawnEnemies : LevelEventTrigger
{
    public SpawnEnemiesEvent eventParameters;

    protected Enemy[] enemies;

    int currentWave;

    bool initialised;

    private PlayerCharacter player;

    private void Awake()
    {
        Init(EVENT_TYPE.SPAWN_ENEMIES);
    }

    private void Start()
    {
        enemies = new Enemy[eventParameters.waves[currentWave].enemyCount];

        player = FindObjectOfType<PlayerCharacter>();
    }

    private void OnValidate()
    {
        if (eventParameters.spawnPoints.Length > 0)
        {
            for (int i = 0; i < eventParameters.spawnPoints.Length; i++)
            {
                if (eventParameters.spawnPoints[i] is null)
                {
                    Debug.LogWarning("There are no transform assigned to one of the spawn points");

                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (initialised && !eventParameters.eventComplete)
        {
            if (currentWave < eventParameters.waves.Length)
            {
                if (AllEnemiesEliminated())
                {
                    eventParameters.waves[currentWave].allEnemiesEliminated = true;

                    InitialiseEnemies();
                }
            }
        }
    }

    public void InitialiseEnemies()
    {
        if (!initialised)
            initialised = true;

        //Clear all existing enemies (done when enemies die)

        //If the current wave of enemies is complete increment the current wave index.

        foreach (EnemyWave wave in eventParameters.waves)
        {
            if (wave.allEnemiesEliminated)
            {
                currentWave++;

                break;
            }
        }

        //If all waves have been defeated, stop the event.
        if (currentWave >= eventParameters.waves.Length)
        {
            eventParameters.eventComplete = true;

            return;
        }

            //Reinitilise Enemies and spawn them in.

            enemies = new Enemy[eventParameters.waves[currentWave].enemyCount];

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector2 pos = eventParameters.spawnPoints[Random.Range(0, eventParameters.spawnPoints.Length)].position;

            GameObject obj = Instantiate(eventParameters.enemyPrefab, pos, Quaternion.identity);

            enemies[i] = obj.GetComponent<Enemy>();
        }
    }

    private bool AllEnemiesEliminated()
    {
        int deadEnemyCount = 0;

        foreach (Enemy e in enemies)
        {
            //if the enemy doesn't exist or is dead.
            if (e is null || e.Health <= 0)
            {
                deadEnemyCount++;

                print(deadEnemyCount);
            }
        }

        if (deadEnemyCount >= enemies.Length  - 1)
        {
            return true;
        }

        return false;
    }
}