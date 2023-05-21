using System;
using UnityEngine;

[Serializable]
public struct Axis
{
    public Wheel[] wheels;
    public bool powered;
    public bool steered;
    public bool isRearWheel;
    public int WheelCount => wheels.Length;

    [Header("If applicable")]
    [Range(0, 30)]
    public float steerRadius;

    public bool CheckGround()
    {
        int isGroundedCount = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].WheelCollider.isGrounded)
                isGroundedCount++;
        }

        return isGroundedCount == wheels.Length;
    }

    public float GetSingleAxisWheelSlip()
    {
        float collectiveSlip = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            collectiveSlip += wheels[i].GetWheelSlip();
        }

        return collectiveSlip;
    }
}