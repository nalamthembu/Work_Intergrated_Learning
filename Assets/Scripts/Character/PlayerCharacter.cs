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

    private void ReadPlayerInput()
    {
        SetArmed(PlayerInput.IsArmed);
        SetShooting(Input.GetMouseButton(0));
        SetAiming(debugAiming ? debugAiming : PlayerInput.IsAiming);
    }

    private void HandleWeapon()
    {
        Weapon.gameObject.SetActive(IsArmed);
    }
}