using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public Axis axes;
}

[System.Serializable]
public struct Axis
{
    public Wheel[] wheels;
    public bool isPowered;
    public bool isSteering;
}