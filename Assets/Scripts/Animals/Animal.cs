using UnityEngine;
public class Animal : MonoBehaviour, IStorable
{
    public GameObject GetGameObject() => gameObject;
    public Transform GetTransform() => transform;
}