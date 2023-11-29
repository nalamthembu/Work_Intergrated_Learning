using UnityEngine;
using System.Collections.Generic;

[
    RequireComponent
    (
        typeof(VehicleInput),
        typeof(VehicleEngine),
        typeof(VehicleTransmission)
    )
]

[
    RequireComponent
    (
        typeof(Rigidbody),
        typeof(VehicleInventory)
    )
]

public class Vehicle : MonoBehaviour
{
    //TO-DO : ABS ON WHEELS.

    public Axis[] axes;

    public bool ABSEnabled = true;

    private Rigidbody rigidBody;

    private VehicleInput input;

    [SerializeField] float downForce = 500;

    readonly private List<Axis> poweredAxis = new();
    readonly private List<Wheel> allWheels = new();
    public List<Axis> PoweredAxis { get { return poweredAxis; } }
    public List<Wheel> AllWheels { get { return allWheels; } }
    public float SpeedKMH { get; private set; }
    public VehicleInventory Inventory { get; private set; }
    private void Awake()
    {
        input = GetComponent<VehicleInput>();

        Inventory = GetComponent<VehicleInventory>();

        rigidBody = GetComponent<Rigidbody>();

        rigidBody.mass = Mathf.Round(rigidBody.mass < 800 ? 800 : rigidBody.mass);

        for (int i = 0; i < axes.Length; i++)
        {
            if (axes[i].isPowered)
            {
                poweredAxis.Add(axes[i]);
            }

            for (int j = 0; j < axes[i].wheels.Length; j++)
            {
                allWheels.Add(axes[i].wheels[j]);
            }
        }
    }

    private void LateUpdate()
    {
        ControlSteering();
    }

    private void FixedUpdate()
    {
        rigidBody.AddForce(1000 * downForce * Vector3.down);

        SpeedKMH = rigidBody.velocity.magnitude * 3.6F;

        rigidBody.drag = (input.throttle <= 0) ? 0.1F : 0.05F;

        ControlWheelStiffness();
    }

    private void ControlWheelStiffness()
    {
        for (int i = 0; i < allWheels.Count; i++)
        {
            switch (allWheels[i].WheelPos)
            {
                case WheelPosition.BACK:
                case WheelPosition.MID:

                    float t = SpeedKMH / 60;

                    float fF = Mathf.Lerp(0.5F, 2.0F, t);
                    float sF = Mathf.Lerp(0.1f, 2.0F, t);

                    allWheels[i].SetWheelStiffness(sF, fF);

                    break;

                case WheelPosition.FRONT:

                    float tF = SpeedKMH / 80;

                    float fF_Fw = Mathf.Lerp(.5F, 3.0F, tF);
                    float sF_Fw = Mathf.Lerp(1f, 3.0f, tF);

                    allWheels[i].SetWheelStiffness(sF_Fw, fF_Fw);

                    break;
            }
        }
    }

    public bool IsGrounded()
    {
        foreach (Axis axis in poweredAxis)
        {
            foreach (Wheel w in axis.wheels)
            {
                //IF ANY OF THE WHEELS ARE NOT TOUCHING THE GROUND
                if (!w.IsGrounded) 
                    return false;
            }
        }

        //THIS ASSUMES ALL THE WHEELS ARE TOUCHING THE GROUND.
        return true;
    }

    private void ControlSteering()
    {
        for(int i = 0; i < axes.Length;i++)
        {
            if (!axes[i].isSteering)
                continue;

            for(int j = 0; j < axes[i].wheels.Length; j++)
            {
                if (input.steering > 0)
                {
                    axes[i].wheels[0].SteeringAngle =
                        Mathf.Rad2Deg *
                        Mathf.Atan(2.55F / (axes[i].steerRadius + (1.5f / 2)))
                        * input.steering;

                    axes[i].wheels[1].SteeringAngle =
                        Mathf.Rad2Deg *
                        Mathf.Atan(2.55F / (axes[i].steerRadius - (1.5f / 2)))
                        * input.steering;
                }
                else if (input.steering < 0)
                {
                    axes[i].wheels[0].SteeringAngle =
                        Mathf.Rad2Deg *
                        Mathf.Atan(2.55F / (axes[i].steerRadius - (1.5f / 2)))
                        * input.steering;

                    axes[i].wheels[1].SteeringAngle =
                        Mathf.Rad2Deg *
                        Mathf.Atan(2.55F / (axes[i].steerRadius + (1.5f / 2)))
                        * input.steering;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position + rigidBody.centerOfMass, Vector3.one * 0.15F);
    }

    private void OnValidate()
    {
        if (rigidBody is null)
        rigidBody = GetComponent<Rigidbody>();
    }
}

[System.Serializable]
public struct Axis
{
    public Wheel[] wheels;
    public float steerRadius;
    public bool isPowered;
    public bool isSteering;
}