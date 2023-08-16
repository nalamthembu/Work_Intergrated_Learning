using UnityEngine;

#region BASE_STATE
public class CharacterStates { }
public abstract class BaseState
{
    public abstract void EnterState(StateMachine stateMachine);
    public abstract void UpdateState(StateMachine stateMachine);
    public abstract void ExitState(StateMachine stateMachine);
    public abstract void CheckStateChange(StateMachine stateMachine);
}
#endregion

namespace PlayerStateMachine
{
    #region PLAYER_STATES
    public class PlayerCrouchState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {

        }

        public override void EnterState(StateMachine stateMachine)
        {

        }

        public override void ExitState(StateMachine stateMachine)
        {

        }

        public override void UpdateState(StateMachine stateMachine)
        {

        }
    }

    public class PlayerFallState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {

        }

        public override void EnterState(StateMachine stateMachine)
        {

        }

        public override void ExitState(StateMachine stateMachine)
        {

        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    public class PlayerIdleState : BaseState
    {
        PlayerCharacter player;
        PlayerLocomotionStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine) => stateMachine.DoStateCheck();

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (PlayerLocomotionStateMachine)stateMachine;
            player = machine.PlayerCharacter;
            machine.ResetFeet();
            player.TargetSpeed = 0;
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            CheckStateChange(stateMachine);
            HandleYPosition();
            //DO NOT ROTATE UNLESS AIMING A WEAPON OF SORTS.
        }

        public void HandleYPosition()
        {
            float currentSpeed = player.CurrentSpeed;
            float targetSpeed = 0;

            if (player.IsGrounded)
                player.VelocityY = 0;

            //Set current Speed
            player.CurrentSpeed = Mathf.SmoothDamp
                (
                  currentSpeed,
                  targetSpeed,
                  ref player.SpeedSmoothVelocity,
                  player.SpeedSmoothTime
                );

            //Set Movement Direction
            player.VelocityY += Time.deltaTime * player.Gravity;
            player.Velocity = (player.transform.forward * player.CurrentSpeed) +
                Vector3.up * player.VelocityY;
            player.Controller.Move(player.Velocity * Time.deltaTime);
        }
    }

    public class PlayerJumpState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {
        }

        public override void EnterState(StateMachine stateMachine)
        {
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    public class PlayerLandState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {
        }

        public override void EnterState(StateMachine stateMachine)
        {
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    public class PlayerStopState : BaseState
    {
        PlayerCharacter player;
        PlayerLocomotionStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            machine.DoSwitchState(machine.PlayerIdleState);
            stateMachine.DoStateCheck();
        }

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (PlayerLocomotionStateMachine)stateMachine;
            player = machine.PlayerCharacter;
            machine.DoFeetCheck();
            player.TargetSpeed = 0;
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            CheckStateChange(stateMachine);
            if (player.PlayerInput.InputMagnitude > 0)
                machine.HandleRotation();

            machine.HandlePosition();
        }
    }

    public class PlayerWalkState : BaseState
    {
        PlayerCharacter player;
        PlayerLocomotionStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine) => stateMachine.DoStateCheck();

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (PlayerLocomotionStateMachine)stateMachine;
            player = machine.PlayerCharacter;
            player.TargetSpeed = player.WalkSpeed;
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            CheckStateChange(stateMachine);
            machine.HandlePosition();
                machine.HandleRotation();
        }
    }

    public class PlayerRunState : BaseState
    {
        PlayerCharacter player;
        PlayerLocomotionStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine) => stateMachine.DoStateCheck();

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (PlayerLocomotionStateMachine)stateMachine;
            player = machine.PlayerCharacter;
            player.TargetSpeed = player.RunSpeed; //MOVE FAST
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            CheckStateChange(stateMachine);
            machine.HandlePosition();
                machine.HandleRotation();
        }
    }
    #endregion
}

namespace AIStateMachine
{
    #region AI_STATES
    public class AICrouchState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {

        }

        public override void EnterState(StateMachine stateMachine)
        {

        }

        public override void ExitState(StateMachine stateMachine)
        {

        }

        public override void UpdateState(StateMachine stateMachine)
        {

        }
    }

    public class AIFallState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {

        }

        public override void EnterState(StateMachine stateMachine)
        {

        }

        public override void ExitState(StateMachine stateMachine)
        {

        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    public class AIIdleState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {
        }

        public override void EnterState(StateMachine stateMachine)
        {
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    public class AIJumpState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {
        }

        public override void EnterState(StateMachine stateMachine)
        {
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    public class AILandState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {
        }

        public override void EnterState(StateMachine stateMachine)
        {
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    public class AIStopState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {
        }

        public override void EnterState(StateMachine stateMachine)
        {
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    public class AIWalkState : BaseState
    {
        public override void CheckStateChange(StateMachine stateMachine)
        {
        }

        public override void EnterState(StateMachine stateMachine)
        {
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }
    #endregion
}