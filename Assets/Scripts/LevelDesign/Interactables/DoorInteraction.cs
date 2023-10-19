using UnityEngine;

//Place this on a door transform.
public class DoorInteraction : MonoBehaviour, IInteractable
{
    const string PLAYER_TAG = "Player";

    DOOR_STATE doorState;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            PlayerInput playerInput = other.GetComponent<PlayerInput>();

            if (playerInput.Interact)
            {
                TriggerInteractable();
            }
        }
    }

    public void TriggerInteractable()
    {
        print("This would trigger a door interactable");

        switch(doorState)
        {
            //This assumes the door is already open.
            case DOOR_STATE.OPEN:

                //PUT LOGIC FOR CLOSING DOORS IN HERE

                //Switch to closed door state.
                doorState = DOOR_STATE.CLOSED;

                break;

                //This assumes the door is closed.
            case DOOR_STATE.CLOSED:

                //PUT LOGIC FOR OPENING DOORS IN HERE


                //Switch to open door state.
                doorState = DOOR_STATE.OPEN;
                break;
        }
    }
}

public enum DOOR_STATE
{
    OPEN,
    CLOSED
}