using UnityEngine;

[CreateAssetMenu(fileName = "Mission Scriptable", menuName = "Game/Mission Scriptable")]
public class MissionScriptable : ScriptableObject
{
    public Mission[] missions;
}