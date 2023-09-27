using UnityEngine;


[CreateAssetMenu(fileName = "Animal", menuName = "Game/Animals/AnimalData")]
public class AnimalScriptable : ScriptableObject
{
    [Header("Stats")]
    new public string name = "Animal Name";
    public int health = 10;
    public float walkSpeed = 10f;
    public float runSpeed = 20f;
    public AnimalType animalType;
    public GameObject pawprint;

    [Header("Capturing stats")]
    public int shotsBeforeDazed = 1;
    public float detectionRange = 15f;


}

public enum AnimalType
{
    CARNIVORE,
    HERBIVORE
}