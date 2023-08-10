using UnityEngine;

//Used in realtime (during gameplay), manages the mission
public class MissionManager : MonoBehaviour
{
    public MissionScriptable missionScriptable;
    public int currentMission; //TO-DO : SAVE THIS NUMBER FOR THE PLAYER PROGRESSION.

    //r  - REALTIME (Used during gameplay)
    Mission rMission;
    Objective rObjective;
    int rCurrentObjective = -1;

    private void Awake()
    {
        rMission = missionScriptable.missions[currentMission];
    }

    private void Start() => NextObjective();

    public void NextObjective()
    {
        rCurrentObjective++;
        rObjective = rMission.objectives[rCurrentObjective];
        HUDManager.instance.ShowSubtitles(rObjective.instructionToPlayer, 5F);
    }
}