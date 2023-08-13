using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Animals_BaseState
{
    float timer = 0;

    public override void EnterState(Animal_StateManager animal)
    {
        animal.isIdle = true;
        //animal.animalAnimator.SetBool("IsInIdle", true);
        Debug.Log("Animal is in idle");
        animal.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public override void ExitState(Animal_StateManager animal)
    {
        animal.isIdle = false;
        //animal.animalAnimator.SetBool("IsInIdle", false);
        Debug.Log("Animal is not in idle");
        animal.GetComponent<MeshRenderer>().material.color = Color.white;
        timer = 0;
    }

    public override void OnRangeEnter(Animal_StateManager animal, GameObject thing)
    {
        if(thing.CompareTag("Player"))
        {
            Debug.Log("Player is in range");
           
        }
    }

    public override void UpdateState(Animal_StateManager animal)
    {
        if(timer >= 5f)
        {
            animal.SwitchState(animal.wander);
        }
        timer += Time.deltaTime;
    }

}
