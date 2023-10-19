using UnityEngine;

//Any script that is interactable must inherit this.
//Please use DoorInteraction.cs as an example of an interactable script.
public interface IInteractable
{
    public abstract void TriggerInteractable();

    public abstract void OnTriggerStay(Collider other);
}