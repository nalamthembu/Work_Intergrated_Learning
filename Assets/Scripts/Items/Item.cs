using UnityEngine;

[
    RequireComponent
    (
        typeof(SphereCollider)
    )
]

public class Item : MonoBehaviour, IStorable
{   
    public GameObject GetGameObject() => gameObject;
    public Transform GetTransform() => transform;
    public bool IsPickedUp { get; private set; }

    new private SphereCollider collider;

    protected Character itemOwner;

    private void OnValidate()
    {
        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPickedUp)
            return;

        PlayerCharacter player = other.GetComponent<PlayerCharacter>();

        if (player is not null)
        {
            IsPickedUp = true;

            itemOwner = player;

            if (this is Weapon weapon)
            {
                transform.SetParent(player.Animator.GetBoneTransform(HumanBodyBones.RightHand));

                weapon.RigidBody.isKinematic = true;

                player.SetWeapon(weapon);

                Collider[] colliders = weapon.GetComponents<Collider>();

                for(int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = false;
                }

                weapon.gameObject.SetActive(false);
            }

            if (collider is not null)
            collider.enabled = false;
        }
    }

    public void Drop()
    {
        IsPickedUp = false;
        itemOwner = null;
        transform.SetParent(null);
    }
}