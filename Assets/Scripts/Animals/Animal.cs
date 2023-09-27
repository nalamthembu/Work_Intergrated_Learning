using Unity.VisualScripting;
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

    [SerializeField] private FieldOfView fieldOfView;

    public float TargetSpeed { get; set; }
    public float CurrentSpeed { get; set; }
    [HideInInspector] public float SpeedSmoothVelocity;
    public float SpeedSmoothTime { get { return speedSmoothTime; } }
    public float WanderTime { get { return wanderTime = Random.Range(1f,5f); } }

    public PlayerCharacter Player { get; private set; }

    public bool PlayerInRange { get; private set; }
    public bool IsKnockedOut { get; private set; }
    public bool IsInDanger { get; set; }
    public bool IsDazed { get; set; }

    private int shotCount;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.SetHealth(animalData.health);
        fieldOfView = GetComponent<FieldOfView>();
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

    public void DisableAnimal()
    {
        gameObject.SetActive(false);
    }

    public void CreatePawPrints()
    {
        GameObject pawprint = Instantiate(animalData.pawprint, new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), Quaternion.identity);
        pawprint.GetComponent<Pawprint>().SetAnimal(this);
    }

    #region PHYSICS FUNCTIONS (OnTrigger, OnCollision)

    private void OnCollisionEnter(Collision collision)
    {
        //IF ANIMAL IS HIT BY DART, INCR. SHOT COUNT.
        if (collision.collider.CompareTag("Dart"))
        {
            shotCount++;

            //IF SHOT COUNT > SHOTS_BEFORE_DAZED, ANIMAL SHOULD BE DAZED.
            if (shotCount >= animalData.shotsBeforeDazed)
            {
                IsDazed = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
        }

        //ONLY CONSIDER A THREAD IF YOU'RE NOT DAZED
        if (!(shotCount >= animalData.shotsBeforeDazed))
        {
            //IF THE BULLET ENTERS AND LEAVES THE TRIGGER, THEN THE PLAYER MISSED
            if (other.CompareTag("Dart"))
            {
                //print("Dart has missed the animal");
                IsInDanger = true;
            }
        }
    }

    #endregion
}