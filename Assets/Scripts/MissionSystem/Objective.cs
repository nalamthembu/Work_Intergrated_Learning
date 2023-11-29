using System;
using UnityEngine;

//Instances of this struct are created in the mission.cs script
[Serializable]
public struct Objective
{
    public string name;
    public string instructionToPlayer;
    public ObjectiveType type;
    public bool IsComplete { get; set; }

    [Header("These will only work if they are relevant")]
    public Vector3 TargetPosition;
    [Range(1, 100)] public float minDistFromTarget;
    public string AnimalToFind;
    public GameObject objectToSpawnForPlayer;
}

public enum ObjectiveType
{
    EnterVehicle,
    Explore,
    SearchAndRescue,
    DriveToLocation,
    SaveAnimal,
    ReturnToHQ,
    Neutralise,
    NeutraliseAnimal,
    Breed,
    TrackAnimal,
    //All the different types of object, example : search and rescue, drive to location, find an animal.
    //TO-DO : ADD DIFFERENT OBJECTIVES.
}