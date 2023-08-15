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
        float wanderTimer = 0;

        float distanceFromTargetLocation;

        AnimalBehaviouralStateMachine machine;

        Animal animal;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            
            machine.CheckForDanger();
            
            if (wanderTimer <= 0)
            {
                machine.DoSwitchState(machine.animalIdleState);
            }
        }

        public override void EnterState(StateMachine stateMachine)
        {
            Debug.Log("Animal is Wandering");
            
            machine = (AnimalBehaviouralStateMachine)stateMachine;

            animal = machine.animal;
            
            wanderTimer = animal.WanderTime;


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
                {
                    machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalWalkState);
                }
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
            machine.CheckForDanger();
            
            if (idleTimer <= 0)
            {
                machine.DoSwitchState(machine.animalWanderingState);
            }
        }

        public override void EnterState(StateMachine stateMachine)
        {
            Debug.Log("Entered Idle State");
            idleTimer = 3f;
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

    public class AnimalRunAwayState : BaseState
    {
        private float runningAwayTimer = 5f;
        float timer = 0;
        AnimalBehaviouralStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            machine.CheckForDanger();
        }

        public override void EnterState(StateMachine stateMachine)
        {
            timer = 0;
            machine = (AnimalBehaviouralStateMachine)stateMachine;
            Debug.Log("Animal is Running Away");
            machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalRunState);
            machine.LocomotionStateMachine.GoToPosition(machine.GetRandomSafePosition(machine.animal.transform.position, 30f));
        }

        public override void ExitState(StateMachine stateMachine)
        {
            Debug.Log("Animal is away from danger");
        }

        public override void UpdateState(StateMachine stateMachine)
        {

            if(timer >= runningAwayTimer && machine.animal.PlayerInRange == true)
            {
                machine.LocomotionStateMachine.GoToPosition(machine.GetRandomSafePosition(machine.animal.transform.position, 30f));
                timer = 0;
            }
            else if (timer >= runningAwayTimer && machine.animal.PlayerInRange == false)
            {
                Debug.Log("Ive ran away");
                machine.DoSwitchState(machine.animalIdleState);
            }

            timer += Time.deltaTime;
        }
    }

    #endregion

}