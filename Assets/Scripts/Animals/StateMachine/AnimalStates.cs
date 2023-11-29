using Unity.VisualScripting;
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
            
            machine = (AnimalBehaviouralStateMachine)stateMachine;

            animal = machine.Animal;
            
            wanderTimer = animal.WanderTime;

            machine.LocomotionStateMachine.GoToPosition(machine.GetRandomNavSphere(machine.Animal.transform.position, 25F));

            machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalWalkState);
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            CheckStateChange(stateMachine);

            wanderTimer -= Time.deltaTime;

            distanceFromTargetLocation = machine.LocomotionStateMachine.GetDistanceFromTargetPosition();

            if (distanceFromTargetLocation < .5F)
            {
                machine.LocomotionStateMachine.GoToPosition(machine.GetRandomNavSphere(machine.Animal.transform.position, 25F));

                if (machine.LocomotionStateMachine.CurrentState is not AnimalLocomotionStates.AnimalWalkState)
                {
                    machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalWalkState);
                }
            }
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
            machine = (AnimalBehaviouralStateMachine)stateMachine;
            idleTimer = 5f;
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

    public class AnimalFleeState : BaseState
    {
        AnimalBehaviouralStateMachine machine;

        float fleeTimer;

        const int MAX_FLEE_DURATION = 10;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            machine.CheckForDanger();
        }

        public override void EnterState(StateMachine stateMachine)
        {
            Debug.Log("Flee");

            if (machine is null)
                machine = (AnimalBehaviouralStateMachine)stateMachine;

            //RUN IF YOU'RE IN DANGER & GO TO A "SAFE" LOCATION
            machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalRunState);
            machine.LocomotionStateMachine.GoToPosition(machine.GetRandomNavSphere(machine.Animal.transform.position, 100.0f));
            machine.Animal.CreatePawPrints();
        }

        public override void ExitState(StateMachine stateMachine)
        {
            //YOU ARE NO LONGER IN DANGER IF YOU MOVE PAST THIS STATE.

            machine.Animal.IsInDanger = false;

            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            fleeTimer += Time.deltaTime;

            if (fleeTimer >= MAX_FLEE_DURATION)
            {
                fleeTimer = 0;

                //GO BACK TO WANDERING
                machine.DoSwitchState(machine.animalWanderingState);
            }
            else
            {
                //IF WE'RE CLOSE ENOUGH TO THE SAFE LOCATION AND WE'RE STILL IN DANGER.
                if (machine.LocomotionStateMachine.HasReachedDestination)
                {
                    //GO TO A "SAFE" LOCATION
                    machine.LocomotionStateMachine.GoToPosition(machine.GetRandomNavSphere(machine.Animal.transform.position, 100.0f));
                }
            }
        }
    }

    public class AnimalKnockedOutState : BaseState
    {
        AnimalBehaviouralStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            return;
        }

        public override void EnterState(StateMachine stateMachine)
        {
            if (machine is null)
                machine = (AnimalBehaviouralStateMachine)stateMachine;

            Debug.Log("Knocked Out");

            //DON'T MOVE
            machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalIdleState);

            machine.Animal.transform.eulerAngles += Vector3.forward * 180;
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            return;
        }
    }

    public class AnimalDazedState : BaseState
    {
        float dazedTimer;

        const int MAX_DAZED_DURATION = 10;

        AnimalBehaviouralStateMachine machine;

        public override void CheckStateChange(StateMachine stateMachine)
        {
            return;
        }

        public override void EnterState(StateMachine stateMachine)
        {
            if (machine is null)
                machine = (AnimalBehaviouralStateMachine)stateMachine;

            Debug.Log("Dazed");

            //MAKE THE ANIMAL MOVE SLOWER WHEN ITS DAZED
            //TO-DO : DAZED WALKING STATE ('DRUNKEN' ANIMAL WALK ANIMATIONS)
            machine.LocomotionStateMachine.DoSwitchState(machine.LocomotionStateMachine.animalWalkState);
        }

        public override void ExitState(StateMachine stateMachine)
        {
            return;
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            //IF WE'RE CLOSE ENOUGH TO THE SAFE LOCATION AND WE'RE STILL IN DANGER.
            if (machine.LocomotionStateMachine.HasReachedDestination)
            {
                //GO TO A "SAFE" LOCATION
                machine.LocomotionStateMachine.GoToPosition(machine.GetRandomNavSphere(machine.Animal.transform.position, 100.0f));
            }

            dazedTimer += Time.deltaTime;

            if (dazedTimer >= MAX_DAZED_DURATION)
            {
                //IF THE TIMER RUNS OUT THE ANIMAL SHOULD FINALLY PASS OUT.

                machine.DoSwitchState(machine.animalKnockedOutState);

                dazedTimer = 0;
            }
        }
    }

    #endregion

}