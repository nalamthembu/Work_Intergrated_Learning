using UnityEngine;

public abstract class Animals_BaseState
{
    public abstract void EnterState(Animal_StateManager animal);

    public abstract void UpdateState(Animal_StateManager animal);

    public abstract void OnRangeEnter(Animal_StateManager animal, GameObject thing);
}
