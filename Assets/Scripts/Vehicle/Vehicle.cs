using UnityEngine;

[
    RequireComponent
    (
        typeof(VehicleInput),
        typeof(VehicleEngine),
        typeof(VehicleTransmission)
    )
]

[
    RequireComponent
    (
        typeof(Rigidbody)
    )
]

public class Vehicle : MonoBehaviour
{
    public Axis axes;

    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        rigidBody.mass = Mathf.Round(rigidBody.mass < 800 ? 800 : rigidBody.mass);
    }
}

[System.Serializable]
public struct Axis
{
    public Wheel[] wheels;
    public bool isPowered;
    public bool isSteering;
}