using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField][Range(0,100)] float health;

    public float Health { get { return health; } }
    public float Armour { get; private set; }

    public bool IsDead { get { return Health <= 0 && Armour <= 0; } }

    public void SetHealth(float newValue) => health = newValue;
    public void SetArmour(float newValue) => Armour = newValue;
}