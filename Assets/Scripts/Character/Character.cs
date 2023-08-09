using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class Character : MonoBehaviour
{
    [SerializeField][Range(1, 10)] protected float walkSpeed = 2;
    [SerializeField][Range(1, 10)] protected float runSpeed = 7;
    [SerializeField][Range(1, 10)] protected float jumpHeight = 2;
    [SerializeField][Range(0, 01)] protected float speedSmoothTime;

    protected bool isGrounded;

    public float TargetSpeed { get; set; }
    public float WalkSpeed { get { return walkSpeed; } }
    public float RunSpeed { get { return runSpeed; } }
    public float CurrentSpeed { get; set; }
    public bool IsGrounded { get { return isGrounded; } }
    public float SpeedSmoothTime { get { return speedSmoothTime; } }
    [HideInInspector] public float SpeedSmoothVelocity;


    private HealthComponent healthComponent;

    public virtual void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
    }

    public void Teleport(Vector3 position, Quaternion rotation) => transform.SetPositionAndRotation(position, rotation);
    public void TakeDamage(float damageAmount)
    {
        if (healthComponent.IsDead)
            return;

        if (healthComponent.Armour >= 0)
        {
            if (healthComponent.Armour - damageAmount <= 0)
            {
                healthComponent.SetArmour(0);
                return;
            }

            healthComponent.SetArmour(healthComponent.Armour - damageAmount);
        }

        if (healthComponent.Health - damageAmount <= 0)
        {
            healthComponent.SetHealth(0);
            return;
        }
    }
    public void Revive(float revivalRate) => healthComponent.SetHealth(healthComponent.Health + revivalRate * Time.deltaTime);
}