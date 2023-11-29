using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Area[] areas;

    [SerializeField] DebugWorld debugWorld;

    [SerializeField] AnimalPopulationCopulation animalPopulation;

    public Area[] Areas { get { return areas; } }
    public AnimalPopulationCopulation AnimalPopulationCopulation { get { return animalPopulation; } }

    private float timeOfDay = 0;

    private int daysPassed = 0;

    private const int TWENTY_FOUR_MINUTES = 1440;

    public static WorldManager Instance;

    public int DaysPassed { get { return daysPassed; } }

    //For organisation, so the hierachy isn't cluttered.
    public Transform AnimalParentTransform { get; private set; }

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitialiseAnimals();
        InitialiseAnimalParentTransform();
    }

    private void Start()
    {
        for (int i = 0; i < areas.Length; i++)
            areas[i].Start();

        animalPopulation.SpawnAnimals();
    }

    public bool GetPointOnNavMesh(Vector3 center, out Vector3 result)
    {
        for (int i = 0; i < 50; i++)
        {
            Vector3 randomPoint = center + (Vector3)Random.insideUnitCircle * Random.Range(50, 200);

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;

        return false;
    }

    private void InitialiseAnimalParentTransform() => AnimalParentTransform = new GameObject("AnimalParent").transform;

    private void OnValidate()
    {
        animalPopulation.OnValidate();

        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].OnValidate();
        }
    }

    private void InitialiseAnimals()
    {
        for (int i = 0; i < areas.Length; i++)
        {
            for (int j = 0; j < areas[i].animalPopulation.Length; j++)
            {
                areas[i].animalPopulation[j].Awake();
            }
        }
    }

    private void Update()
    {
        HandleTimeOfDay();

        if (debugWorld.enabled)
            debugWorld.Update();
    }

    private void HandleTimeOfDay()
    {
        timeOfDay += Time.deltaTime;

        //A full day in this game is 24 minutes IRL.
        if (timeOfDay >= TWENTY_FOUR_MINUTES)
        {
            daysPassed++;

            timeOfDay = 0;

            animalPopulation.Multiply();
        }
    }

    public string GetTimeOfDay() => HelperMethods.StopWatchFormattedTime(timeOfDay);

    public void IncrementDay() => timeOfDay += 86400;

    public void OnDrawGizmosSelected()
    {
        //Drawout area.
        int i = 0;
        foreach (Area a in areas)
        {
            Gizmos.color = i switch
            {
                0 => Color.green,
                1 => Color.blue,
                2 => Color.red,
                3 => Color.yellow,
                4 => Color.gray,
                5 => Color.magenta,
                6 => Color.cyan,
                _ => Color.white,
            };

            Gizmos.DrawWireSphere(a.areaCentre, a.radius);

            i++;
        }
    }
}


[System.Serializable] 
public struct Area
{
    public string name;
    public Vector3 areaCentre; //World Space Coords of the centre of the area.
    [Range(10, 1000)] public float radius; //We're opting for a radius rather than set bounds.
    public AnimalPopulation[] animalPopulation;
    private List<Animal> allAnimals;

    public int totalAnimalCount;

    private int carnivoreCount;

    public void Start()
    {
        allAnimals = new();
    }

    public void AddAnimal(Animal animal)
    {
        allAnimals.Add(animal);
        totalAnimalCount = allAnimals.Count;
    }

    private void CountCarnivores()
    {
        for (int i = 0; i<animalPopulation.Length; i++)
        {
            switch (animalPopulation[i].animalType)
            {
                case AnimalType.Carnivore:
                    carnivoreCount = animalPopulation[i].count;
                    break;
            }
        }
    }

    //Population Control
    public void KillAnimals()
    {
        CountCarnivores();

        for (int i = 0; i < animalPopulation.Length; i++)
        {
            switch (animalPopulation[i].animalType)
            {
                case AnimalType.Herbivore:

                    //Assume all the carnivores kill a single herbivore.
                    animalPopulation[i].count -= carnivoreCount;

                    //Kill animals that are too old
                    for (int j = 0; j < allAnimals.Count; j++)
                    {
                        //TO-DO : (KEON) KILL THE OLD ANIMALS.
                        //if animal is too old
                        break;

                        allAnimals.Remove(allAnimals[j]);

                        animalPopulation[i].count--;
                    }

                    //Make sure we don't go into negative animals (that's not supposed to be possible)
                    if (animalPopulation[i].count < 0)
                        animalPopulation[i].count = 0;
                    break;
            }
        }
    }

    //Validate data (The area radius should never be less than or equal to 0)
    public void OnValidate()
    {
        if (radius < 0)
        {
            radius = Mathf.Abs(radius);
        }

        if (radius == 0)
        {
            radius = 1;
        }
    }
    //YOuve been Porzinga'd
    //Let me know if the player is in the area.
    public bool IsPlayerInArea()
    {
        Collider[] colliders = Physics.OverlapSphere(areaCentre, radius);
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out PlayerCharacter _))
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public struct AnimalPopulationCopulation
{
    [Header("Distances from Player")]
    [Range(1, 100)] public float minDistance;
    [Range(1, 100)] public float maxDistance;
    [Range(1, 100)] public float animalSpawnRadius;

    private int animalPopulationCount;

    //Validate data (Make sure min distance isn't more than max distance.
    public void OnValidate()
    {
        if (minDistance > maxDistance)
        {
            minDistance = maxDistance;
            maxDistance++;
        }
    }

    // will check the population of animals in the area,
    // will check how many animals of each species there are
    // if there are 2 or more animals, they will reproduce and create 1 more of that species for each pair of animal there is
    public void Multiply()
    {
        for (int k = 0; k < WorldManager.Instance.Areas.Length; k++)
        {
            for (int i = 0; i < WorldManager.Instance.Areas[k].animalPopulation.Length; i++)
            {
                if (WorldManager.Instance.Areas[k].animalPopulation[i].count >= 2)
                {
                    WorldManager.Instance.Areas[k].animalPopulation[i].count
                        += WorldManager.Instance.Areas[k].animalPopulation[i].count / 2;

                    SpawnAnimals();

                    WorldManager.Instance.Areas[k].KillAnimals();
                }
            }
        }
    }

    public void SpawnAnimalsInSpecificArea(Vector3 position)
    {
        for (int i = 0; i < WorldManager.Instance.Areas.Length; i++)
        {
            Area area = WorldManager.Instance.Areas[i];

            foreach (AnimalPopulation animalPopulation in area.animalPopulation)
            {
                //If theres no animals or if there is no prefab assigned to the scriptable, skip the iteration.
                if (animalPopulation.count <= 0 || animalPopulation.animalData.prefab == null)
                    continue;

                //optimisation
                int maxAnimals = 50 / area.animalPopulation.Length;

                if (animalPopulationCount >= maxAnimals)
                    return;

                for (int j = 0; j < maxAnimals; j++)
                {
                    //Get A Specified Position within the area away from the camera
                    Vector3 specifiedPosition = position;

                    //Check with the navMesh if that's even possible.
                    if (WorldManager.Instance.GetPointOnNavMesh(specifiedPosition, out Vector3 result))
                    {
                        //the final position is that position behind the camera and the animals are spread by the animalSpawnRadius.
                        specifiedPosition = result;

                        //Instantiate Animal at that position.
                        GameObject animalGO = Object.Instantiate(animalPopulation.animalData.prefab, specifiedPosition, Quaternion.identity, WorldManager.Instance.AnimalParentTransform);

                        Animal animal = animalGO.GetComponent<Animal>();

                        area.AddAnimal(animal);

                        animalPopulationCount++;
                    }
                }
            }
        }
    }

    //Spawn Animals based on animal population count in world manager.
    public void SpawnAnimals()
    {
        for(int i = 0; i < WorldManager.Instance.Areas.Length; i++)
        {
            Area area = WorldManager.Instance.Areas[i];

            foreach (AnimalPopulation animalPopulation in area.animalPopulation)
            {
                //Don't spawn animals if the player is not in the area (coz we won't seem em')
                if (!area.IsPlayerInArea())
                    break;

                //If theres no animals or if there is no prefab assigned to the scriptable, skip the iteration.
                if (animalPopulation.count <= 0 || animalPopulation.animalData.prefab == null)
                    continue;

                //optimisation
                int maxAnimals = 50 / area.animalPopulation.Length;

                if (animalPopulationCount >= maxAnimals)
                    return;

                for (int j = 0; j < maxAnimals; j++)
                {
                    //Get A Random Position within the area away from the camera
                    Vector3 playerPosition = PlayerCharacter.Instance.transform.position;
                    Vector3 cameraForward = CameraController.Instance.transform.forward;

                    //Get player position, then point behind the camera, get a random distance in that area.
                    Vector3 randomPositionAwayFromPlayer = playerPosition + -(cameraForward * Random.Range(minDistance, maxDistance));
                    Vector3 finalPosition;

                    //Check with the navMesh if that's even possible.
                    if (WorldManager.Instance.GetPointOnNavMesh(randomPositionAwayFromPlayer, out Vector3 result))
                    {
                        //the final position is that position behind the camera and the animals are spread by the animalSpawnRadius.
                        finalPosition = result;

                        //Instantiate Animal at that position.
                        GameObject animalGO = Object.Instantiate(animalPopulation.animalData.prefab, finalPosition, Quaternion.identity, WorldManager.Instance.AnimalParentTransform);

                        Animal animal = animalGO.GetComponent<Animal>();

                        area.AddAnimal(animal);

                        animalPopulationCount++;
                    }
                }
            }
        }
    }
}


[System.Serializable]
public struct AnimalPopulation
{
    public string name;
    public AnimalScriptable animalData;
    public AnimalType animalType;
    public int count;

    public void Awake()
    {
        if (animalData is null)
        {
            Debug.Log("There is no scriptable object assigned to this animal!");
            return;
        }

        name = animalData.name;
        animalType = animalData.animalType;
    }
}

[System.Serializable]
public struct DebugWorld
{
    public bool enabled;
    
    public void Update()
    {
        if (enabled)
        {
            //Speed Up Time
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Time.timeScale += 0.5F;
                Debug.Log("Debug Time : " + Time.timeScale);

                if (Time.timeScale >= 36)
                    Debug.Log("This is the maximum speed (36x)");
            }

            if (Input.GetKeyDown(KeyCode.Equals))
            {
                WorldManager.Instance.IncrementDay();
                Debug.Log("Skipping ahead to next day");
            }

            //Playback Normally
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Time.timeScale = 1;
                Debug.Log("Debug Time : " + Time.timeScale);
            }

            //Slow Time Down
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Time.timeScale -= 0.25F;
                Debug.Log("Debug Time : " + Time.timeScale);

                if (Time.timeScale <= 0.25F)
                    Debug.Log("This is the minumum speed (0.25x)");
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                WorldManager.Instance.AnimalPopulationCopulation.Multiply();
            }
        }
    }
}