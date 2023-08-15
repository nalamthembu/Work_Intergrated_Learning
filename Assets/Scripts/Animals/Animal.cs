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

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.SetHealth(animalData.health);
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
}