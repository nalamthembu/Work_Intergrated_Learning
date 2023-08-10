using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


[RequireComponent(typeof(NavMeshAgent))]
public class Animal_StateManager : MonoBehaviour
{
    // initialising states and other important things
    Animals_BaseState currentState;
    [SerializeField] public AnimalScriptable animal;
    public Idle idle = new Idle();
    public Wandering wander = new Wandering();
    public Startled startled = new Startled();
    public RunningAway running = new RunningAway();
    public KnockedOut KO = new KnockedOut();
    public Attacking attack = new Attacking();
    public NavMeshAgent animals;

    // bools and such
    public bool playerInRange = false;
    public bool gotShot = false;
    public bool isTired = false;
    public bool outOfDanger = true;

    // animals stats 
    public string name = "Name";
    public int health = 10;
    public float speed = 10f;
    public int torporLevel = 1;
    public float detectionRange = 15f;

    // important objects in game
    [SerializeField] public GameObject player;
    public Vector3 currentPos;
    public Vector3 playerPos;


    void Start()
    {
        // initialise stats
        name = animal.name;
        health = animal.health;
        speed = animal.speed;
        torporLevel = animal.torporLevel;
        detectionRange = animal.detectionRange;
        // sets up important information for the nav mesh agent
        animals = gameObject.GetComponent<NavMeshAgent>();
        animals.speed = speed;
        currentState = idle;
        // enters into a default state when the gam start
        currentState.EnterState(this);
    }

    void Update()
    {
        
    }

    // Will use this when something enters the range of the animal
    private void OnTriggerEnter(Collider other)
    {
        currentState.OnRangeEnter(this, other.gameObject);
    }

    // will be used to switch between states
    public void SwitchState(Animals_BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
