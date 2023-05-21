using UnityEngine;

public class VehicleInput : MonoBehaviour
{
    [Range(0, 1)]
    public float throttle;
    [Range(0, 1)]
    public float brake;
    [Range(-1, 1)]
    public float steering;
    [Range(0, 1)]
    public float handbrake;
    [Range(1, 0)]
    public float clutch;
    [Header("Reverse")]
    public bool reverse;
}
