using UnityEngine;
using static GameStrings;
using PlayerStateMachine;

public class PlayerLocomotionStateMachine : StateMachine
{
    #region STATE_OBJECTS
    public PlayerIdleState PlayerIdleState = new();
    public PlayerWalkState PlayerWalkState = new();
    public PlayerRunState PlayerRunState = new();
    public PlayerJumpState PlayerJumpState = new();
    public PlayerFallState PlayerFallState = new();
    public PlayerLandState PlayerLandState = new();
    public PlayerStopState PlayerStopState = new();
    public PlayerCrouchState PlayerCrouchState = new();
    #endregion

    private PlayerInput playerInput;
    public PlayerCharacter PlayerCharacter { get; private set; }

    private void Start()
    {
        PlayerCharacter = GetComponent<PlayerCharacter>();
        playerInput = GetComponent<PlayerInput>();
        animator = PlayerCharacter.Animator;
        lFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        currentState = PlayerIdleState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        DoAnimation();
        currentState.UpdateState(this);
    }

    public override void DoSwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this);
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

    public void DoAnimation()
    {
        float targetRotation = Mathf.Atan2(playerInput.InputDir.x, playerInput.InputDir.y) * Mathf.Rad2Deg;

        animator.SetFloat(INPUT_MAGNITUDE, playerInput.InputMagnitude);
        animator.SetFloat(CURRENT_SPEED, PlayerCharacter.CurrentSpeed);
        animator.SetFloat(INPUT_X, playerInput.InputDir.x);
        animator.SetFloat(INPUT_Y, playerInput.InputDir.y);
        animator.SetBool(IS_RUNNING, playerInput.IsRunning);
        animator.SetBool(IS_JUMPING, playerInput.IsJumping);
        animator.SetFloat(TARGET_ROTATION, targetRotation);
    }

    public void HandlePosition()
    {
        float currentSpeed = PlayerCharacter.CurrentSpeed;
        float targetSpeed = PlayerCharacter.TargetSpeed;

        if (PlayerCharacter.IsGrounded)
            PlayerCharacter.VelocityY = 0;

        //Check if jumping, switch states if true.

        //Set current Speed
        PlayerCharacter.CurrentSpeed = Mathf.SmoothDamp
            (
              currentSpeed,
              targetSpeed,
              ref PlayerCharacter.SpeedSmoothVelocity,
              PlayerCharacter.SpeedSmoothTime
            );

        //Set Movement Direction
        PlayerCharacter.VelocityY += Time.deltaTime * PlayerCharacter.Gravity;
        PlayerCharacter.Velocity = (PlayerCharacter.transform.forward * PlayerCharacter.CurrentSpeed) + 
            Vector3.up * PlayerCharacter.VelocityY;
        PlayerCharacter.Controller.Move(PlayerCharacter.Velocity * Time.deltaTime);
    }

    public void HandleRotation()
    {
        //Calculate Rotation
        PlayerCharacter.TargetRotation =
            Mathf.Atan2(playerInput.InputDir.x, playerInput.InputDir.y) *
            Mathf.Rad2Deg + PlayerCharacter.MainCamera.eulerAngles.y;

        //Set Rotation
        PlayerCharacter.transform.eulerAngles = Vector3.up *
            Mathf.SmoothDampAngle
            (
                PlayerCharacter.transform.eulerAngles.y,
                PlayerCharacter.TargetRotation,
                ref PlayerCharacter.turnSpeedVelocity,
                PlayerCharacter.TurnSmoothTime
            );
    }

    public override void DoStateCheck()
    {
        if (PlayerCharacter.IsGrounded)
        {
            //CHECK IF CURRENT ANIMATION IS AIR ANIM, DO FEET CHECK THEN RETURN.

            //CHECK IF THE PLAYER IS NOT FALLING RN
            if (PlayerCharacter.IsGrounded)
            {
                //CHECK IF PLAYER WAS PREVIOUSLY MOVING
                //IF THERE'S INPUT FROM THE PLAYER
                if (playerInput.InputMagnitude != 0)
                {
                    //SWITCH TO RELEVANT STATE
                    if (playerInput.IsRunning)
                        DoSwitchState(PlayerRunState);
                    else
                        DoSwitchState(PlayerWalkState);
                }
                else
                {
                    if (PlayerCharacter.CurrentSpeed > 0 && currentState != PlayerIdleState)
                    {
                        DoSwitchState(PlayerStopState);
                    }
                }
            }
            else
            {
                DoSwitchState(PlayerFallState);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Color stateColor = Color.clear;

        if (currentState is not null)
        {
            #region IF_STATES
            if (currentState is PlayerIdleState)
                stateColor = Color.grey;

            if (currentState is PlayerWalkState)
                stateColor = Color.white;

            if (currentState is PlayerRunState)
                stateColor = Color.green;

            if (currentState is PlayerJumpState)
                stateColor = Color.yellow;

            if (currentState is PlayerFallState)
                stateColor = Color.magenta;

            if (currentState is PlayerLandState)
                stateColor = Color.blue;

            if (currentState is PlayerStopState)
                stateColor = Color.red;

            if (currentState is PlayerCrouchState)
                stateColor = Color.black;
            #endregion
        }

        Gizmos.color = stateColor;

        Gizmos.DrawWireCube(transform.position + transform.up * 2, Vector3.one * 0.15F);
    }
}