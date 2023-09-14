using UnityEngine;

[
    RequireComponent
    (
        typeof(CharacterController),
        typeof(PlayerInput)
    )
]

[
    RequireComponent
    (
        typeof(PlayerLocomotionStateMachine)
    )
]

public class PlayerCharacter : Character
{
    [SerializeField][Range(-50, 0)] float gravity;
    [SerializeField][Range(00, 01)] protected float turnSmoothTime;
    public float VelocityY { get; set; }
    public Vector3 Velocity { get; set; }
    public float Gravity { get { return gravity; } }
    [HideInInspector] public float turnSpeedVelocity;
    private CharacterController controller;
    public CharacterController Controller { get { return controller; } }
    public float TargetRotation { get; set; }
    public Transform MainCamera { get; private set; }
    public CameraController CameraController { get; private set; }
    [HideInInspector] public float TurnSmoothTime { get { return turnSmoothTime; } }
    public PlayerInput PlayerInput { get; private set; }

    #region DEBUG
    public bool debugAiming = false;
    #endregion

    public override void Awake()
    {
        base.Awake();
        controller = GetComponent<CharacterController>();
        MainCamera = Camera.main.transform;
        CameraController = MainCamera.GetComponentInParent<CameraController>();
        PlayerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        isGrounded = controller.isGrounded | Physics.Raycast(transform.position, Vector3.down, 0.125f);
    }

    private void Update()
    {
        ReadPlayerInput();

        if (Weapon is not null)
            HandleWeapon();
    }

    private void PickUpKOdAnimal()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1F);

        foreach (Collider c in colliders)
        {
            if (c.TryGetComponent(out Animal animal))
            {
                if (animal.isKnockedOut)
                {
                    if (CurrentVehicle == null)
                    {
                        HUDManager.instance.ShowNotification("Enter the truck!");
                        return;
                    }

                    if (Vector3.Distance(animal.transform.position, CurrentVehicle.transform.position) < 4F)
                    {
                        //Put animal in vehicle inventory
                        HUDManager.instance.ShowNotification("+1 Animal added to the vehicle inventory");
                        animal.DisableAnimal();
                        CurrentVehicle.Inventory.AddItem(animal.name, animal);
                        print("placing animal in, inventory");
                    }
                    else
                    {
                        HUDManager.instance.ShowNotification("You are too far from the vehicle");
                        print("animal is too far from the vehicle");
                    }

                    print("found an incapacitated animal");

                    return;
                }    
            }
        }
    }

    private void ReadPlayerInput()
    {
        SetArmed(PlayerInput.IsArmed);
        SetShooting(Input.GetMouseButton(0));
        SetAiming(debugAiming ? debugAiming : PlayerInput.IsAiming);
        
        if (PlayerInput.PickUpAnimal)
        {
            PickUpKOdAnimal();
        }
    }

    private void HandleWeapon()
    {
        Weapon.gameObject.SetActive(IsArmed);
    }
}