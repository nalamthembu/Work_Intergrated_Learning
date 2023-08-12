using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : Animals_BaseState
{
    private float timer = 0;
    public float wanderTimer = 5f;
    public override void EnterState(Animal_StateManager animal)
    {
        animal.isWandering = true;
        //animal.animalAnimator.SetBool("IsInWandering", true);
        Debug.Log("Is wandering");
    }

    public override void ExitState(Animal_StateManager animal)
    {
        throw new System.NotImplementedException();
    }

    public override void OnRangeEnter(Animal_StateManager animal, GameObject thing)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(Animal_StateManager animal)
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Debug.Log("Animal is wandering around the area");
            Vector3 newPos = animal.GetRandomNavSphere(animal.currentPos, animal.wanderRadius, -1);
            animal.NavMeshAgent.SetDestination(newPos);
            timer = 0;
        }
    }

    
}
