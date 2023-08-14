using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : Animals_BaseState
{
    private float timer = 0;
    public float wanderTimer = 7f;
    public override void EnterState(Animal_StateManager animal)
    {
        animal.isWandering = true;
        //animal.animalAnimator.SetBool("IsInWandering", true);
        animal.GetComponent<MeshRenderer>().material.color = Color.blue;

        Debug.Log("Animal is wandering around the area");
        Vector3 newPos = animal.GetRandomNavSphere(animal.currentPos, animal.wanderRadius, -1);
        animal.NavMeshAgent.SetDestination(newPos);
    }

    public override void ExitState(Animal_StateManager animal)
    {
        animal.isWandering = false;
        animal.GetComponent<MeshRenderer>().material.color = Color.white;
        timer = 0;
    }

    public override void OnRangeEnter(Animal_StateManager animal, GameObject thing)
    {
        Debug.Log("OnRangeEnter was called");
    }

    public override void UpdateState(Animal_StateManager animal)
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            animal.SwitchState(animal.idle);
        }
    }
}