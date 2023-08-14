using UnityEngine;

public class AnimalStates { }
public abstract class AnimalBaseState : BaseState
{
    public override abstract void EnterState(StateMachine stateMachine);

    public override abstract void UpdateState(StateMachine stateMachine);

    public override abstract void ExitState(StateMachine stateMachine);
}

namespace AnimalLocomotionStates
{
    #region LocomotionStates
    public class AnimalWalkState : AnimalBaseState
    {
        Animal animal;
        AnimalLocomotionStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            return;
        }

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (AnimalLocomotionStateMachine)stateMachine;
            animal = machine.Animal;
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            machine.UpdateSpeed(animal.animalData.walkSpeed);
        }
    }

    public class AnimalRunState : AnimalBaseState
    {
        Animal animal;
        AnimalLocomotionStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            return;
        }

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (AnimalLocomotionStateMachine)stateMachine;
            animal = machine.Animal;
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            machine.UpdateSpeed(animal.animalData.runSpeed);
        }
    }

    public class AnimalIdleMovementState : AnimalBaseState
    {
        AnimalLocomotionStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            return;
        }

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (AnimalLocomotionStateMachine)stateMachine;
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            machine.UpdateSpeed(0F);
            CheckStateChange(machine);
        }
    }

    public class AnimalStopState : AnimalBaseState
    {
        AnimalLocomotionStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            return;
        }

        public override void EnterState(StateMachine stateMachine)
        {
            machine = (AnimalLocomotionStateMachine)stateMachine;
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            machine.UpdateSpeed(0);
        }
    }
    #endregion
}

namespace AnimalBehaviourStates
{
    #region BehaviouralStates
    public class AnimalWanderingState : BaseState
    {
        float wanderTimer;

        float distanceFromTargetLocation;

        AnimalBehaviouralStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            if (wanderTimer <= 0)
            {
                machine.DoSwitchState(machine.animalIdleState);
            }
        }

        public override void EnterState(StateMachine stateMachine)
        {
            wanderTimer = Random.Range(5F, 15F);
            machine = (AnimalBehaviouralStateMachine)stateMachine;
            machine.LocomotionStateMachine.GoToPosition(machine.GetRandomNavSphere(machine.animal.transform.position, 25F));
            machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalWalkState);
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            wanderTimer -= Time.deltaTime;

            distanceFromTargetLocation = machine.LocomotionStateMachine.GetDistanceFromTargetPosition();

            if (distanceFromTargetLocation < .5F)
            {
                machine.LocomotionStateMachine.GoToPosition(machine.GetRandomNavSphere(machine.animal.transform.position, 25F));
                if (machine.LocomotionStateMachine.CurrentState is not AnimalLocomotionStates.AnimalWalkState)
                    machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalWalkState);
            }
            else if (distanceFromTargetLocation > 10F)
            {
                machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalRunState);
            }


            CheckStateChange(stateMachine);
        }
    }

    public class AnimalIdleState : BaseState
    {
        float idleTimer;
        AnimalBehaviouralStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            if (idleTimer <= 0)
            {
                machine.DoSwitchState(machine.animalWanderingState);
            }
        }

        public override void EnterState(StateMachine stateMachine)
        {
            Debug.Log("Entered Idle State");
            idleTimer = Random.Range(5F, 15F);
            machine = (AnimalBehaviouralStateMachine)stateMachine;
            machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalIdleState);
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            idleTimer -= Time.deltaTime;
            CheckStateChange(stateMachine);
        }
    }

    #endregion

}