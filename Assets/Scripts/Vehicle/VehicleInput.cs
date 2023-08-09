using UnityEngine;

//This script keeps track of inputs for the vehicle, it will be used by other scripts under the vehicle object.
public class VehicleInput : MonoBehaviour
{
    [Range(0, 1)]   public float throttle;
    [Range(0, 1)]   public float brake;
    [Range(0, 1)]   public float handbrake;
    [Range(-1, 1)]  public float steering;
}