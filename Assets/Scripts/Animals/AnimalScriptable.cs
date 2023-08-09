using UnityEngine;


[CreateAssetMenu(fileName = "Animal", menuName = "Game/Animals")]
public class AnimalScriptable : ScriptableObject
{
    [Header("Stats")]
    new public string name = "Animal Name";
    public int health = 10;
    public float speed = 10f;

    [Header("Capturing stats")]
    public int torporLevel = 1;
    public float detectionRange = 15f;

}