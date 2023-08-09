using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Animals")]
public class Animal : ScriptableObject
{
    [Header("Stats")]
    public string name = "Animal Name";
    public int health = 10;
    public float speed = 10f;

    [Header("Capturing stats")]
    public int torporLevel = 1;
    public float detectionRange = 15f;

}
