using UnityEngine;
using static GameStrings;
using CharacterSituationStateMachine;

public class CharacterSituationalStateMachine : StateMachine
{
    #region STATE_OBJECTS
    public CharacterArmedState ChrArmedState = new();
    public CharacterUnArmedState ChrUnArmedState = new();
    public CharacterInVehicleState ChrInVehState = new();
    #endregion

    public Character Character { get; private set; }

    private void Start()
    {
        Character = GetComponent<Character>();
        animator = Character.Animator;
        lFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);

        currentState = ChrUnArmedState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public override void DoSwitchState(BaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void SwitchToInVehicleState(Vehicle v)
    {
        currentState = ChrInVehState;
        ChrInVehState.SetVehicle(v);
        CurrentState.EnterState(this);
    }

    public void ResetFeet()
    {
        animator.SetBool(LU, false);
        animator.SetBool(RU, false);
    }

    public void DoFeetCheck()
    {
        if (Vector3.Dot(lFoot.position, rFoot.position) > 0)
        {
            animator.SetBool(LU, false);
            animator.SetBool(RU, true);
        }
        else
        {
            animator.SetBool(LU, true);
            animator.SetBool(RU, false);
        }
    }

    public void OnDrawGizmos()
    {
        Color stateColor = Color.clear;

        if (currentState is not null)
        {
            #region IF_STATES
            if (currentState is CharacterUnArmedState)
                stateColor = Color.white;

            if (currentState is CharacterArmedState)
                stateColor = Color.red;

            if (currentState is CharacterInVehicleState)
                stateColor = Color.green;
            #endregion
        }

        Gizmos.color = stateColor;

        Gizmos.DrawWireCube(transform.position + transform.up * 4, Vector3.one * 0.15F);
    }
}