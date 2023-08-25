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

    public Transform cameraFocus;

    private void Awake()
    {
        vehicleInput = GetComponent<VehicleInput>();
    }

    private void Update()
    {
        if (PlayerInputEnabled)
        {
            vehicleInput.throttle = Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : Mathf.Lerp(vehicleInput.throttle, 0, Time.deltaTime);
            vehicleInput.brake = Input.GetAxis("Vertical") < 0 ? -Input.GetAxis("Vertical") : Mathf.Lerp(vehicleInput.brake, 0, Time.deltaTime);
            vehicleInput.steering = Input.GetAxis("Horizontal");
            vehicleInput.handbrake = Input.GetAxis("Jump");
        }
    }
}