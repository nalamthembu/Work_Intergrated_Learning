using CharacterSituationStateMachine;
using UnityEngine;

//Used in realtime (during gameplay), manages the mission
public class MissionManager : MonoBehaviour
{
    public MissionScriptable missionScriptable;
    public int currentMission; //TO-DO : SAVE THIS NUMBER FOR THE PLAYER PROGRESSION.

    public GameObject ObjectiveHighlighter;

    //r  - REALTIME (Used during gameplay)
    Mission rMission;
    Objective rObjective;
    int rCurrentObjective = -1;

    CharacterSituationalStateMachine playerSituationalMachine;

    //Specific to One Objective
    bool hasNotifiedPlayerToEnterVehicle;
    bool hasNotifiedPlayerToTrackAnimal;

    private void Awake()
    {
        rMission = missionScriptable.missions[currentMission];
    }

    private void Start()
    {
        NextObjective();

        playerSituationalMachine = PlayerCharacter.Instance.GetComponent<CharacterSituationalStateMachine>();

        ObjectiveHighlighter.SetActive(false);
    }

    private void Update()
    {
        ProcessCurrentObjective();
    }

    private void ProcessCurrentObjective()
    {
        switch (rObjective.type)
        {
            case ObjectiveType.EnterVehicle:

                if (hasNotifiedPlayerToEnterVehicle && playerSituationalMachine.CurrentState is CharacterInVehicleState)
                {
                    //Don't set IsComplete to true on rObjective -> its permenant, that's not supposed to happen.
                    NextObjective();
                }

                if (!hasNotifiedPlayerToEnterVehicle)
                {
                    HUDManager.instance.ShowSubtitles(rObjective.instructionToPlayer, 5.0F);
                    hasNotifiedPlayerToEnterVehicle = true;
                }
                break;

            case ObjectiveType.DriveToLocation:

                hasNotifiedPlayerToEnterVehicle = false;

                ObjectiveHighlighter.transform.position = rObjective.TargetPosition;

                ObjectiveHighlighter.SetActive(true);

                float distFromLocation = Vector3.Distance(PlayerCharacter.Instance.transform.position, rObjective.TargetPosition);

                if (distFromLocation <= rObjective.minDistFromTarget)
                {
                    //You're close enough to the target location.

                    ObjectiveHighlighter.SetActive(false);

                    NextObjective();
                }

                break;

            case ObjectiveType.TrackAnimal:

                if (!hasNotifiedPlayerToTrackAnimal)
                {
                    HUDManager.instance.ShowSubtitles("To track the " + rObjective.AnimalToFind + " use your mini map.", 5.0F);

                    //Spawn the animals so we have a chance at finding em'.
                    Vector3 pos = CameraController.Instance.transform.position + CameraController.Instance.transform.forward * Random.Range(25, 50);
                    WorldManager.Instance.AnimalPopulationCopulation.SpawnAnimalsInSpecificArea(pos);
                    hasNotifiedPlayerToTrackAnimal = true;
                }

                //Physics.OverlapSphere(PlayerCharacter.Instance.transform.position)

                break;
        }
    }

    public void NextObjective()
    {
        rCurrentObjective++;
        rObjective = rMission.objectives[rCurrentObjective];
        HUDManager.instance.ShowSubtitles(rObjective.instructionToPlayer, 5F);
    }
}