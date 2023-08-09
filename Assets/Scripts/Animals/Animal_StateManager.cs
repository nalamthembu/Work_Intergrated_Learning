using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // bools and such
    public bool playerInRange = false;
    public bool gotShot = false;
    public bool isTired = false;
    public bool outOfDanger = true;

    // animals stats 
    public string name = "name";
    public int health = 10;
    public float speed = 10f;
    public int torporLevel = 1;
    public float detectionRange = 15f;

    // important objects in game
    [SerializeField] public GameObject player;


    void Start()
    {
        name = animal.name;
        health = animal.health;
        speed = animal.speed;
        torporLevel = animal.torporLevel;
        detectionRange = animal.detectionRange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
