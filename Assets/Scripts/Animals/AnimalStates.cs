using UnityEngine;


public class AnimalStates { }
public abstract class AnimalBaseState
{
    public abstract void EnterState(StateMachine animal);

    public abstract void UpdateState(StateMachine animal);

    public abstract void ExitState(StateMachine animal);

    public abstract void OnRangeEnter(StateMachine animal, GameObject thing);
}

namespace AnimalStateMachine
{
    #region LocomotionStates

    public class AnimalWalkState : AnimalBaseState
    {
        private Animal animal;
        private AnimalLocomotionStateMachine machine;

        public override void EnterState(StateMachine animal)
        {
            machine = (AnimalLocomotionStateMachine)animal;
            this.animal = machine.Animal;
            this.animal.TargetSpeed = this.animal.animalData.walkSpeed;
        }

        public override void ExitState(StateMachine animal)
        {
            return;
        }

        public override void OnRangeEnter(StateMachine animal, GameObject thing)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState(StateMachine animal)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AnimalRunState : AnimalBaseState
    {
        public override void EnterState(StateMachine animal)
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState(StateMachine animal)
        {
            throw new System.NotImplementedException();
        }

        public override void OnRangeEnter(StateMachine animal, GameObject thing)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState(StateMachine animal)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AnimalIdleState : AnimalBaseState
    {
        float timer = 0;

        public override void EnterState(StateMachine animal)
        {
            Debug.Log("Animal is in idle");
            animal.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        public override void ExitState(StateMachine animal)
        {
            Debug.Log("Animal is not in idle");
            animal.GetComponent<MeshRenderer>().material.color = Color.white;
            timer = 0;
        }

        public override void OnRangeEnter(StateMachine animal, GameObject thing)
        {
            if (thing.CompareTag("Player"))
            {
                Debug.Log("Player is in range");

            }
        }

        public override void UpdateState(StateMachine animal)
        {
            
        }
    }

    public class AnimalStopState : AnimalBaseState
    {
        public override void EnterState(StateMachine animal)
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState(StateMachine animal)
        {
            throw new System.NotImplementedException();
        }

        public override void OnRangeEnter(StateMachine animal, GameObject thing)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState(StateMachine animal)
        {
            throw new System.NotImplementedException();
        }
    }

    #endregion

    #region BehaviourStates


    #endregion
}