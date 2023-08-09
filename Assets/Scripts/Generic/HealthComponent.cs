using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float Health { get; private set; }
    public float Armour { get; private set; }

    public bool IsDead { get { return Health <= 0 && Armour <= 0; } }

    public void SetHealth(float newValue) => Health = newValue;
    public void SetArmour(float newValue) => Armour = newValue;
}