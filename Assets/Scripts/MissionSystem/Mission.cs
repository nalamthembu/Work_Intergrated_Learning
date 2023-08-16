using System;

//Instances of the mission struct are created in the mission manager
[Serializable]
public struct Mission
{
    public string name;
    public Objective[] objectives;
    public bool IsComplete { get; set; }
}