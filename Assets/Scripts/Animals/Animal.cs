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

    [SerializeField] [Range(0.1f, 10f)] float wanderTime = 0.25f;

    public float TargetSpeed { get; set; }
    public float CurrentSpeed { get; set; }
    [HideInInspector] public float SpeedSmoothVelocity;
    public float SpeedSmoothTime { get { return speedSmoothTime; } }
    public float WanderTime { get { return wanderTime; } }

    public PlayerCharacter Player { get; private set; }

    public bool PlayerInRange = false;

    public bool gotShot = false;

    public bool firedAt = false;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.SetHealth(animalData.health);
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
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player is out of range");
            PlayerInRange = false;
        }
    }

    public void CreatePawPrints()
    {
        Instantiate(animalData.pawprint, new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), Quaternion.identity);
    }
}