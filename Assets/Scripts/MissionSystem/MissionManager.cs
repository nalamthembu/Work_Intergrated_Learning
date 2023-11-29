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
    bool hasNotifiedPlayerToNeutraliseAnimal;
    Animal animalToNeutralise;
    [SerializeField] bool isInTutorial;
    bool animalGotAwayInTutorialMode;

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

                Collider[] colliders = Physics.OverlapSphere(PlayerCharacter.Instance.transform.position, 15.0F);

                foreach (Collider col in colliders)
                {
                    if (col.TryGetComponent<Animal>(out var animal))
                    {
                        NextObjective();
                        animalToNeutralise = animal;
                        hasNotifiedPlayerToTrackAnimal = false;
                        break;
                    }
                }

                break;


            case ObjectiveType.NeutraliseAnimal:

                if (animalToNeutralise is not null)
                {
                    //Give the player a gun.
                    Instantiate(rObjective.objectToSpawnForPlayer, PlayerCharacter.Instance.transform.position, Quaternion.identity);

                    //TAG THE ANIMAL SO THE PLAYER CAN SEE WHICH ONE THEY NEED.

                    if (isInTutorial)
                    {
                        //Make them really fast.
                        animalToNeutralise.TargetSpeed = 100;

                        float distanceFromAnimal = Vector3.Distance(animalToNeutralise.transform.position, PlayerCharacter.Instance.transform.position);

                        if (distanceFromAnimal > 50.0F && !animalGotAwayInTutorialMode)
                        {
                            //Aww man, the animal got away.

                            animalGotAwayInTutorialMode = true;

                            HUDManager.instance.ShowSubtitles("Aw man, the " + animalToNeutralise.animalData.name + " got away! Don't worry though, this was just a test run, lets do it again for real this time.", 15.0F);
                        }
                    }

                    if (animalToNeutralise.IsKnockedOut)
                    {
                        NextObjective();

                        break;
                    }
                }

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