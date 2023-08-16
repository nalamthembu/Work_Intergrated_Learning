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

    private Character itemOwner;

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
                
                transform.SetPositionAndRotation
                (
                    weapon.WeaponData.restingPosition,
                    Quaternion.Euler(weapon.WeaponData.restingRotation)
                 );
            }
        }
    }

    private void Drop()
    {
        IsPickedUp = false;
        itemOwner = null;
        transform.SetParent(null);
    }
}