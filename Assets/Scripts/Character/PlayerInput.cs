using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float InputMagnitude { get; private set; }
    public Vector2 InputDir { get; set; }

    public bool IsRunning { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsArmed { get; private set; }
    public bool IsShooting { get; private set; } //Might not work as well as I want it to.
    public bool IsAiming { get; private set; }

    PlayerCharacter playerCharacter;

    private void Awake() => playerCharacter = GetComponent<PlayerCharacter>();

    private void Update()
    {
        InputDir = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        InputMagnitude = InputDir.normalized.magnitude;
        IsRunning = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            IsCrouching = !IsCrouching;

        IsJumping = Input.GetAxis("Jump") > 0;

        //toggle equipping
        if (Input.GetKeyDown(KeyCode.Tab))
            IsArmed = !IsArmed;

        IsAiming = Input.GetMouseButton(1);
    }

    public float GetMouseX(float mouseSensitivity) => Input.GetAxis("Mouse X") * mouseSensitivity;
    public float GetMouseY(float mouseSensitivity) => Input.GetAxis("Mouse Y") * mouseSensitivity;
}