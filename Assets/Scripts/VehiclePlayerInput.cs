using UnityEngine;

[RequireComponent(typeof(VehicleInput))]
public class VehiclePlayerInput : MonoBehaviour
{
    private VehicleInput vehicleInput;
    private void Awake() => vehicleInput = GetComponent<VehicleInput>();

    Vector2 directionalInput;

    bool handBrake;

    bool reverse;

    private void Update()
    {
        ProcessInput();
        PassValuesToVehicleInput();
    }

    private void PassValuesToVehicleInput()
    {
        if (vehicleInput is null)
            return;

        vehicleInput.throttle = (directionalInput.y >= 0) ? directionalInput.y : 0;
        vehicleInput.brake = (directionalInput.y <= 0) ? Mathf.Abs(directionalInput.y) : 0;
        vehicleInput.handbrake = handBrake ? Mathf.Lerp(vehicleInput.handbrake, 1, Time.deltaTime * 5F) : 0;
        vehicleInput.steering = directionalInput.x;
        vehicleInput.reverse = reverse;
    }

    private void ProcessInput()
    {
        directionalInput = new()
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };

        handBrake = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.R))
            reverse = !reverse;
    }
}