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

    private NavMeshAgent navMeshAgent;

    public BaseState CurrentState { get { return currentState; } }

    private void Awake()
    {
        Animal = GetComponent<Animal>();
        navMeshAgent = GetComponent<NavMeshAgent>();
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

        navMeshAgent.speed = Animal.CurrentSpeed;
    }

    public float GetDistanceFromTargetPosition() => navMeshAgent.remainingDistance;
    private void Update()
    {
        currentState.UpdateState(this);
        //Debug.Log(currentState);
    } 
    public void GoToPosition(Vector3 position) => navMeshAgent.SetDestination(position);
}