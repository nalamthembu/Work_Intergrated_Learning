using UnityEngine;
using UnityEngine.AI;

[
    RequireComponent
    (
        typeof(HealthComponent),
        typeof(AnimalLocomotionStateMachine),
        typeof(AnimalBehaviouralStateMachine)
    )
]

[
    RequireComponent
    (
        typeof(NavMeshAgent)
    )
]

public class Animal : MonoBehaviour, IStorable
{
    public AnimalScriptable animalData;

    private HealthComponent healthComponent;

    public GameObject GetGameObject() => gameObject;
    public Transform GetTransform() => transform;

    [SerializeField] [Range(0.1F, 2F)] float speedSmoothTime = 0.25F;

    [SerializeField] private float wanderTime = 0;

    public int torporLevel = 0;
    public int timesShot = 0;
    public float TargetSpeed { get; set; }
    public float CurrentSpeed { get; set; }
    [HideInInspector] public float SpeedSmoothVelocity;
    public float SpeedSmoothTime { get { return speedSmoothTime; } }
    public float WanderTime { get { return wanderTime = Random.Range(1f,5f); } }

    public PlayerCharacter Player { get; private set; }

    public bool PlayerInRange = false;

    public bool gotShot = false;

    public bool firedAt = false;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.SetHealth(animalData.health);
        torporLevel = animalData.torporLevel;
    }

    private void Start()
    {
        Player = FindObjectOfType<PlayerCharacter>();
    }
    public void Teleport(Vector3 position, Quaternion rotation) => transform.SetPositionAndRotation(position, rotation);

    public void TakeDamage(float damageAmount)
    {
        if (healthComponent.IsDead)
            return;

        if (healthComponent.Health - damageAmount <= 0)
        {
            healthComponent.SetHealth(0);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Im In Danger");
            PlayerInRange = true;
        }
        else if(other.tag == "Dart")
        {
            Debug.Log("Someone tried to shoot me");
            firedAt = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player is out of range");
            PlayerInRange = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Dart")
        {
            Debug.Log("Got shot");
            gotShot = true;
            timesShot += 1;
        }
    }
    public void CreatePawPrints()
    {
        GameObject pawprint = Instantiate(animalData.pawprint, new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), Quaternion.identity);
        pawprint.GetComponent<Pawprint>().SetAnimal(this);
    }
}