using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected BaseState currentState;

    protected Animator animator;

    protected Transform lFoot, rFoot;

    public BaseState CurrentState { get { return currentState; } }

    public virtual void DoStateCheck()
    {
        //Do something.
    }

    public virtual void DoSwitchState(BaseState state)
    {
        print("no switch state implimentation!");
    }
}