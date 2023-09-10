using UnityEngine;

[

    RequireComponent
    (
        typeof(VehicleInput)
    )
]

public class PlayerVehicleInput : MonoBehaviour
{
    private VehicleInput vehicleInput;
    public bool PlayerInputEnabled { get; set; }

    private Rigidbody rb;

    public CameraSettings cameraSettings;

    private void Awake()
    {
        vehicleInput = GetComponent<VehicleInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (PlayerInputEnabled)
        {
            vehicleInput.throttle = Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : Mathf.Lerp(vehicleInput.throttle, 0, Time.deltaTime);
            vehicleInput.brake =Input.GetAxis("Vertical") < 0 ? -Input.GetAxis("Vertical") : Mathf.Lerp(vehicleInput.brake, 0, Time.deltaTime);
            vehicleInput.steering = Input.GetAxis("Horizontal");
            vehicleInput.handbrake = Input.GetAxis("Jump");
        }
    }

    public void ApplyEBrake()
    {
        vehicleInput.handbrake = 1;
    }
}

[System.Serializable]
public struct CameraSettings
{
    public float distance;
    public float FOV;
    public Transform target;
}