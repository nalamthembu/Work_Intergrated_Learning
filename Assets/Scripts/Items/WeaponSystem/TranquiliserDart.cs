using UnityEngine;

public class TranquiliserDart : Bullet
{
    public override void OnCollisionEnter(Collision collision)
    {
        transform.forward = -collision.contacts[0].normal;
    }
}