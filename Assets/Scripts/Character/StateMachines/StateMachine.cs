using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected BaseState currentState;

    protected Animator animator;

    protected Transform lFoot, rFoot;

    public virtual void DoStateCheck()
    {
        //Do something.
    }
}