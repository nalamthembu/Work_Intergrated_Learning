using UnityEngine;

[
    RequireComponent
    (
        typeof(Rigidbody)
    )
]

public class Bullet : Item
{
    public float bulletSpeed { get; private set; }

    private void LateUpdate()
    {
        transform.position += bulletSpeed * Time.deltaTime * transform.forward;
        
        //TO-DO : If the weight isn't enough to pull the dart down, add downforce.
        //TO-DO : Add additional forces for more difficulty (wind).
    }

    public virtual void OnCollisionEnter(Collision collision) => Destroy(gameObject);
}