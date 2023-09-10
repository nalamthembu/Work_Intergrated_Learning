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

    public bool HitSomething { get; private set; }

    private Rigidbody rigidBody;

    public void Initialise(float bulletSpeed)
    {
        this.bulletSpeed = bulletSpeed;
        rigidBody.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

        Destroy(gameObject, 10);
    }

    private void Awake() => rigidBody = GetComponent<Rigidbody>();

    private void LateUpdate()
    {
        //TO-DO : If the weight isn't enough to pull the dart down, add downforce.
        //TO-DO : Add additional forces for more difficulty (wind).

        //Spin Bullet
        if (!HitSomething)
            transform.eulerAngles += 1000F * Time.deltaTime * Vector3.forward;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        rigidBody.isKinematic = true;
        HitSomething = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + -transform.forward);
    }
}