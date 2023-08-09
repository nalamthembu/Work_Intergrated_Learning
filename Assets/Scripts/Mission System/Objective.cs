using System;

//Instances of this struct are created in the mission.cs script
[Serializable]
public struct Objective
{
    public string name;
    public string instructionToPlayer;
    public ObjectiveType type;
    public bool IsComplete { get; set; }
}

public enum ObjectiveType
{
    //All the different types of object, example : search and rescue, drive to location, find an animal.
    //TO-DO : ADD DIFFERENT OBJECTIVES.
}