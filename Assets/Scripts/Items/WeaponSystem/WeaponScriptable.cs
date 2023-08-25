using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Game/Weapon/WeaponData")]
public class WeaponScriptable : ScriptableObject
{
    new public string name;
    public float range = 50; //in metres
    public float maxClip = 30; //max ammo in one single clip
    public float damage = 10;
    public float fireRate = 5;

    public GameObject bulletPrefab;

    public FIRE_TYPE fireType;

    [Header("Resting")]
    public Vector3 restingPosition;
    public Vector3 restingRotation;

    [Header("Aiming")]
    public Vector3 aimingPosition;
    public Vector3 aimingRotation;
}

public enum FIRE_TYPE
{
    SEMIAUTO,
    AUTO
}