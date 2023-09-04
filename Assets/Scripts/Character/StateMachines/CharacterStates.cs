using UnityEngine;
using static GameStrings;

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

namespace PlayerLocomotionStateMachineSPC
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

namespace CharacterSituationStateMachine
{
    #region SITUATION_STATES
    public class CharacterArmedState : BaseState
    {
        CharacterSituationalStateMachine machine;
        Character character;
        PlayerCharacter playerChar;
        Transform cameraT;
        PlayerInput playerInput;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            if (!character.IsArmed)
                machine.DoSwitchState(machine.ChrUnArmedState);
        }

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (CharacterSituationalStateMachine)stateMachine;
            character = machine.Character;

            playerChar = character.GetComponent<PlayerCharacter>();
            playerInput = playerChar.PlayerInput;
            cameraT = playerChar.MainCamera;
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            HandleAnimations();
            HandleRotation();
            HandlePosition();
            CheckStateChange(stateMachine);
        }

        private void HandleRotation()
        {
            if (playerChar is not null && playerChar.IsAiming)
            {
                playerChar.transform.eulerAngles = Vector3.up * cameraT.parent.eulerAngles.y;
            }
        }

        private void HandlePosition()
        {
            if (playerChar is not null && playerChar.IsAiming)
            {
                playerChar.Velocity =
                    (playerChar.CurrentSpeed * playerInput.InputDir.y * playerChar.transform.forward) +
                    (playerChar.CurrentSpeed * playerInput.InputDir.x * playerChar.transform.right) +
                        Vector3.up * playerChar.VelocityY;
            }
        }

        private void HandleAnimations()
        {
            character.Animator.SetBool(IS_ARMED, character.IsArmed);
            character.Animator.SetBool(IS_AIMING, character.IsAiming);
            character.Animator.SetBool(IS_SHOOTING, character.IsShooting);

            if (character is PlayerCharacter player)
            {
                character.Animator.SetFloat(CAMERA_X, Mathf.Clamp(-player.CameraController.PitchYaw[0], -90, 90));
                character.Animator.SetFloat(INPUT_X, playerInput.InputDir.x);
                character.Animator.SetFloat(INPUT_Y, playerInput.InputDir.y);
            }
        }
    }

    public class CharacterUnArmedState : BaseState
    {
        CharacterSituationalStateMachine machine;
        Character character;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            if (character.IsArmed)
            {
                machine.DoSwitchState(machine.ChrArmedState);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Collider[] cols = Physics.OverlapSphere(machine.Character.transform.position, 1f);

                for (int i = 0; i < cols.Length; i++)
                {
                    Vehicle vehicle = cols[i].GetComponent<Vehicle>();

                    if (vehicle is null)
                        continue;

                    Debug.Log("Found a vehicle");
                    machine.SwitchToInVehicleState(vehicle);
                }
            }
        }

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (CharacterSituationalStateMachine)stateMachine;
            character = machine.Character;
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            HandleAnimations();
            CheckStateChange(stateMachine);
        }

        private void HandleAnimations()
        {
            character.Animator.SetBool(IS_ARMED, character.IsArmed);
            character.Animator.SetBool(IS_AIMING, character.IsAiming);
            character.Animator.SetBool(IS_SHOOTING, character.IsShooting);
        }
    }

    public class CharacterInVehicleState : BaseState
    {
        Vehicle vehicle;
        PlayerVehicleInput input;
        CharacterSituationalStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                machine.DoSwitchState(machine.ChrUnArmedState);
            }
        }

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (CharacterSituationalStateMachine)stateMachine;
            input = vehicle.GetComponent<PlayerVehicleInput>();
            CameraController.instance.SetTarget(input.cameraFocus, 15F, 60);
            input.PlayerInputEnabled = true;
            machine.Character.DisableForVehicle();
            machine.Character.transform.parent = vehicle.transform;
        }

        //BUG : PLAYER DOESN'T EXIT THE VEHICLE.
        public override void ExitState(StateMachine stateMachine)
        {
            machine.Character.EnableOutsideVehicle();
            input.PlayerInputEnabled = false;
            machine.Character.transform.parent = null;
            //TO-DO : LOOK AT PLAYER INSTEAD.
            //CameraController.instance.SetTarget(input.cameraFocus, 15F, 60);
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            CheckStateChange(stateMachine);
        }

        public void SetVehicle(Vehicle vehicle) => this.vehicle = vehicle;
    }
    #endregion
}