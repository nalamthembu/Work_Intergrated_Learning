using AnimalBehaviourStates;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehaviouralStateMachine : StateMachine
{
    public Animal animal { get; private set; }

    public AnimalLocomotionStateMachine LocomotionStateMachine { get; private set; }

    public AnimalWanderingState animalWanderingState = new();
    public AnimalIdleState animalIdleState = new(); //Grazing and such.

    //TO-DO : ADD MORE STATES.

    private void Awake()
    {
        animal = GetComponent<Animal>();
        LocomotionStateMachine = GetComponent<AnimalLocomotionStateMachine>();
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

    private void Update() => currentState.UpdateState(this);

    public Vector3 GetRandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randVector = Random.insideUnitSphere * dist;

        randVector += origin;

        NavMesh.SamplePosition(randVector, out NavMeshHit navHit, dist, -1);

        return navHit.position;
    }
}
