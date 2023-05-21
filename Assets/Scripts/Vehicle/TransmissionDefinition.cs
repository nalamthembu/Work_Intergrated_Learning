using UnityEngine;

[CreateAssetMenu(menuName  = "Game/Vehicle/Transmission Definition", fileName = "TransmissionDefinition")]
public class TransmissionDefinition : ScriptableObject
{
    [Header("Gear count is equal to the number of gear ratios")]
    public float[] gearRatios;

    [Header("Optimal Speeds per gear (KM/H)")]
    public float[] optimalSpeed;

    [Header("Reverse Gear Ration")]
    public float reverseGearRatio;

    public float gearChangeDuration = 0.25F;

    private void OnValidate()
    {
        if (optimalSpeed.Length < gearRatios.Length)
            optimalSpeed = new float[gearRatios.Length];
    }
}
