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

    public class AnimalIdleState : AnimalBaseState
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
            machine.LocomotionStateMachine.GoToPosition(machine.GetRandomNavSphere(machine.animal.transform.position, 5F));
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            wanderTimer -= Time.deltaTime;
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
            idleTimer = Random.Range(5F, 15F);
            machine = (AnimalBehaviouralStateMachine)stateMachine;
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