using UnityEngine;
using UnityEngine.AI;
using AnimalLocomotionStates;

public class AnimalLocomotionStateMachine : StateMachine
{
    #region STATE_OBJECTS
    public AnimalIdleMovementState animalIdleState = new();
    public AnimalWalkState animalWalkState = new();
    public AnimalRunState animalRunState = new();
    public AnimalStopState animalStopState = new();
    #endregion

    public Animal Animal { get; private set; }

    public NavMeshAgent NavMeshAgent { get; private set; }


    public bool HasReachedDestination
    {
        get
        {
            return NavMeshAgent.remainingDistance <= 1.0F;
        }
    }

    private void Awake()
    {
        Animal = GetComponent<Animal>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentState = animalIdleState;
        currentState.EnterState(this);
    }

    public override void DoSwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void UpdateSpeed(float targetSpeed)
    {
        Animal.CurrentSpeed = Mathf.SmoothDamp
            (
                Animal.CurrentSpeed,
                targetSpeed,
                ref Animal.SpeedSmoothVelocity,
                Animal.SpeedSmoothTime
            );

        NavMeshAgent.speed = Animal.CurrentSpeed;
    }

    public float GetDistanceFromTargetPosition() => NavMeshAgent.remainingDistance;

    private void Update()
    {
        currentState.UpdateState(this);
    } 

    public void GoToPosition(Vector3 position) => NavMeshAgent.SetDestination(position);
}