using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptable", menuName = "Game/DialogueScriptable")]

public class DialogueScriptables : ScriptableObject
{
    public CrewMember[] crewMember;
}

[System.Serializable]

public class CrewMember
{
    public string memberName;
    public GenericDialogue[] genDialoog;
    public CurrentDialogue[] currentDialoog;
    public MissionDialogue[] missionDialoog;
}

[System.Serializable]

public class GenericDialogue
{
    public string dialogueName;
    public string dialogueDescription;
}
[System.Serializable]

public class CurrentDialogue
{
    public string dialogueName;
    public string dialogueDescription;
}
[System.Serializable]

public class MissionDialogue
{
    public string dialogueName;
    public string dialogueDescription;
}