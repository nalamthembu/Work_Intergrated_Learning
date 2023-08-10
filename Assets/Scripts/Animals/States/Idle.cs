using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Animals_BaseState
{
    public override void EnterState(Animal_StateManager animal)
    {
        animal.isIdle = true;
        animal.animalAnimator.SetBool("IsInIdle", true);
        Debug.Log("Animal is in idle");
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
        throw new System.NotImplementedException();
    }

}
