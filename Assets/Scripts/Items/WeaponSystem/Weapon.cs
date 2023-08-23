using UnityEngine;

public class Weapon : Item
{
    [SerializeField] WeaponScriptable weaponData;

    public WeaponScriptable WeaponData { get { return weaponData; } }
    public Rigidbody RigidBody { get; private set; }

    float nextTimeToFire = 0;

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
    }

    private void Update() => ProcessOwnerInput();   
    
    private void ProcessOwnerInput()
    {
        if (IsPickedUp && itemOwner.IsAiming)
        {
            //TO-DO : IS SHOOTING CHECK.

            if (itemOwner.IsShooting)
            {
                switch (weaponData.fireType)
                {
                    case FIRE_TYPE.SEMIAUTO:
                        Fire();
                        break;

                    case FIRE_TYPE.AUTO:
                        if (Time.time >= nextTimeToFire)
                        {
                            nextTimeToFire = Time.time + 1.0F / weaponData.fireRate;
                            Fire();
                        }
                        break;
                }
            }
        }
    }

    private void Fire()
    {

    }
}