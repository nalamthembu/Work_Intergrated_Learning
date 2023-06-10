using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VehicleInput))]
[RequireComponent(typeof(Transmission))]
[RequireComponent(typeof(Engine))]
[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
    public VehicleSeat[] seats;

    public Axis[] axes;
    public  VehicleInput input { get; private set; }
    public Transmission transmission { get; private set; }
    public Vector3 localVelocity { get; private set; }
    public Engine engine { get; private set; }
    new private Rigidbody rigidbody;

    public float VehicleSpeedKMPH { get; private set; }
    public bool Grounded { get; private set; }

    [Header("Driving Assistance")]
    public bool ABS;
    private bool ABSActive;

    private void Awake()
    {
        input = GetComponent<VehicleInput>();
        transmission = GetComponent<Transmission>();
        engine = GetComponent<Engine>();
        rigidbody = GetComponent<Rigidbody>();
        transmission.Init();
    }

    private void Start() => InitialiseSeats();

    public void InitialiseSeats()
    {
        if (seats.Length < 0)
            return;

        for (int i = 0; i < seats.Length; i++)
            seats[i].Initialise(this);
    }

    private void FixedUpdate()
    {
        UpdateAxes();
        UpdatePhysics();

        if (ABS)
            CalculateABS();
    }

    private void CalculateABS()
    {
        float absThreshold = .5F;

        for (int i = 0; i < axes.Length; i++)
        {
            for (int j = 0; j < axes[i].wheels.Length; j++)
            {
                Wheel w = axes[i].wheels[j];

                //ABS is only active when the wheels slip too much when the brake is applied.
                ABSActive = w.GetWheelSlip() >= absThreshold && input.brake > 0;

                if (ABSActive)
                    StartCoroutine(ApplyABS(w, absThreshold, 0.25F));
            }
        }
    }

    private IEnumerator ApplyABS(Wheel wheel, float absThreshold, float absFrequency)
    {
        //INFO : ABS basically releases the brake periodically
        //to prevent skidding, which reduces the stopping distance of the vehicle.
        while(wheel.GetWheelSlip() >= absThreshold)
        {
            wheel.SetBrakeTorque(0); //release the brakes
            yield return new WaitForSeconds(absFrequency); //wait for a short period (0.25seconds)
            break;
            //Continue working as normal -> ABSActive will be turned false -> see UpdateAxes();
        }
    }

    private void UpdatePhysics()
    {
        localVelocity = rigidbody.velocity;

        int groundCheck = 0;

        for (int i = 0; i < axes.Length; i++)
        {
            if (axes[i].CheckGround())
                groundCheck++;
        }

        Grounded = groundCheck == axes.Length;

        //Apply DownForce
        rigidbody.AddForce(Vector3.down * 500);

        VehicleSpeedKMPH = Mathf.Floor(rigidbody.velocity.magnitude * 3.6F);
    }

    public float GetAxesWheelSlip()
    {
        float collectiveSlip = 0;

        for(int i =0;i < axes.Length; i++)
            collectiveSlip += axes[i].GetSingleAxisWheelSlip();

        return collectiveSlip / axes.Length;
    }

    public void UpdateAxes()
    {
        for (int i = 0; i < axes.Length; i++)
        {
            //ABS
            Wheel[] wheels = axes[i].wheels;

            if (!ABSActive) //When ABS is inactive
            {
                foreach (Wheel w in wheels)
                {
                    w.SetBrakeTorque(input.brake * 500); //Apply the brakes.
                }
            }

            //Handbrake
            if (axes[i].isRearWheel)
            {
                for (int j = 0; j < wheels.Length; j++)
                {
                    wheels[j].SetBrakeTorque(wheels[j].WheelCollider.brakeTorque + input.handbrake * 5000);
                }
            }

            //Steering
            if (axes[i].steered)
            {
                if (input.steering > 0)
                {
                    wheels[0].WheelCollider.steerAngle =
                        Mathf.Rad2Deg *
                            Mathf.Atan(2.55F / (axes[i].steerRadius + (1.5f / 2)))
                            * input.steering;

                    wheels[1].WheelCollider.steerAngle =
                        Mathf.Rad2Deg *
                            Mathf.Atan(2.55F / (axes[i].steerRadius - (1.5f / 2)))
                            * input.steering;
                }
                else if (input.steering < 0)
                {
                    wheels[0].WheelCollider.steerAngle =
                        Mathf.Rad2Deg *
                            Mathf.Atan(2.55F / (axes[i].steerRadius - (1.5f / 2)))
                            * input.steering;

                    wheels[1].WheelCollider.steerAngle =
                        Mathf.Rad2Deg *
                            Mathf.Atan(2.55F / (axes[i].steerRadius + (1.5f / 2)))
                            * input.steering;
                }
                else
                {
                    wheels[0].WheelCollider.steerAngle = wheels[1].WheelCollider.steerAngle = input.steering;
                }
            }

        }
    }
}