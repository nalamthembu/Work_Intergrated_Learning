using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class Animal : MonoBehaviour, IStorable
{
    public AnimalScriptable animalData;

    private HealthComponent healthComponent;

    public GameObject GetGameObject() => gameObject;
    public Transform GetTransform() => transform;

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