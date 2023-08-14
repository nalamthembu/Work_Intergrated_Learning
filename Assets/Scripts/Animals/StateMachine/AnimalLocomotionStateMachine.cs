using UnityEngine;
using UnityEngine.AI;
using AnimalLocomotionStates;

public class AnimalLocomotionStateMachine : StateMachine
{
    #region STATE_OBJECTS
    public AnimalIdleState animalIdleState = new();
    public AnimalWalkState animalWalkState = new();
    public AnimalRunState animalRunState = new();
    public AnimalStopState animalStopState = new();
    #endregion

    public Animal Animal { get; private set; }

    private NavMeshAgent navMeshAgent;

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
        float currentSpeed = Animal.CurrentSpeed;

        Animal.CurrentSpeed = Mathf.SmoothDamp
            (
                currentSpeed,
                targetSpeed,
                ref Animal.SpeedSmoothVelocity,
                Animal.SpeedSmoothTime
            );
    }

    private void Update() => currentState.UpdateState(this);
    public void GoToPosition(Vector3 position) => navMeshAgent.SetDestination(position);
}