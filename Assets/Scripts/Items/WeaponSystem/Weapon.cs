using UnityEngine;

public class Weapon : Item
{
    [SerializeField] WeaponScriptable weaponData;

    public WeaponScriptable WeaponData { get { return weaponData; } }
}