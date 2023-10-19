using UnityEngine;

[
    RequireComponent
    (
        typeof(BoxCollider)
    )
]

//Every interactables should inherit this.
public class Interactable : MonoBehaviour, IInteractable
{
    new private BoxCollider collider;

    //Can be overriden
    protected virtual void Awake()
    {
        collider = GetComponent<BoxCollider>();

        //Making sure its a trigger
        collider.isTrigger = true;
    }

    public virtual void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public virtual void TriggerInteractable()
    {
        throw new System.NotImplementedException();
    }
}