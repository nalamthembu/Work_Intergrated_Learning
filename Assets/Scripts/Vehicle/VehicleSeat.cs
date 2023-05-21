using System;
using UnityEngine;

[Serializable]
public struct VehicleSeat
{
    public Seat seat;
    public Transform transform;
    public bool SpawnCharacterInSeat;

    public void Initialise(Vehicle vehicle) //Call in Awake
    {
        if (SpawnCharacterInSeat)
        {
            /*
            Transform character = GameInstance.instance.SpawnRandomCharacterAndReturnValue().transform;
            character.SetParent(transform);
            character.localPosition = character.localEulerAngles = Vector3.zero;
            Character component = character.GetComponent<Character>();
            component.vehicle = vehicle;
            */
        }
    }
}

public enum Seat
{
    FRONT_LEFT,
    FRONT_RIGHT,
    BACK_LEFT,
    BACK_RIGHT
}
