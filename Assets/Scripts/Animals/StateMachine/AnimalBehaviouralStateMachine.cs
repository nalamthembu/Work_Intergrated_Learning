using AnimalBehaviourStates;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehaviouralStateMachine : StateMachine
{
    public Animal Animal { get; private set; }

    public AnimalLocomotionStateMachine LocomotionStateMachine { get; private set; }

    public AnimalWanderingState animalWanderingState = new();
    public AnimalIdleState animalIdleState = new(); //Grazing and such.
    public AnimalFleeState animalFleeState = new();
    public AnimalDazedState animalDazedState = new();
    public AnimalKnockedOutState animalKnockedOutState = new();
    //TO-DO : ADD MORE STATES.

    private void Awake()
    {
        Animal = GetComponent<Animal>();
        LocomotionStateMachine = GetComponent<AnimalLocomotionStateMachine>();
    }

    private void Start()
    {
        currentState = animalWanderingState;
        currentState.EnterState(this);
    }

    public override void DoSwitchState(BaseState state)
    {
        //BUG FOUND & FIXED : EXIT STATE WAS NOT BEING CALLED DURING SWITCH.
        if (currentState is not null)
            currentState.ExitState(this);

        currentState = state;
        state.EnterState(this);
    }

    private void Update() 
    {
        currentState.UpdateState(this);
    } 

    // random position in a circle
    public Vector3 GetRandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randVector = Random.insideUnitSphere * dist;

        randVector += origin;

        NavMesh.SamplePosition(randVector, out NavMeshHit navHit, dist, -1);

        return navHit.position;
    }

    public void CheckForDanger()
    {
        //IF PLAYER IS IN RANGE, YOU GET SHOT AT OR HIT, YOU ARE IN DANGER.
        if (Animal.PlayerInRange)
        {
            Animal.IsInDanger = true;
        }

        //IF YOU'RE IN DANGER, SWITCH OVER TO DANGER STATE.
        if (Animal.IsInDanger && currentState != animalFleeState)
            DoSwitchState(animalFleeState);


        //IF YOU SHOULD BE DAZED AND YOU'RE NOT, BE DAZED.
        if (Animal.IsDazed && currentState != animalDazedState)
            DoSwitchState(animalDazedState);
    }

    public void CheckForPrey()
    {
        //TO-DO : IMPLEMENT PREDATOR BEHAVIOUR
    }
}
