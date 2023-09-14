using UnityEngine;

//TO-DO : WILL KEEP TRANSMISSION AUTO FOR NOW.
public class VehicleTransmission : MonoBehaviour
{
    public VehicleTransmissionAndPowerData powerData;

    private int currentGear;

    private VehicleInput input;

    private Vehicle vehicle;

    private VehicleEngine engine;

    public float DrivetrainRPM { get; private set; }
    public int CurrentGear { get { return currentGear; } }

    private bool isInReverse;

    [SerializeField] private float timeToChangeGears;

    private float gearChangeTimer;

    private bool IsChangingGear;

    private void Awake()
    {
        input = GetComponent<VehicleInput>();
        vehicle = GetComponent<Vehicle>();
        engine = GetComponent<VehicleEngine>();
    }

    private void LateUpdate()
    {
        CalculateDrivetrainRPM();
        DistributePowerAmongWheels();
        ProcessGearChanges();
    }

    private void CalculateDrivetrainRPM()
    {
        float sumRPM = 0;

        int numOfWheels = vehicle.AllWheels.Count;

        foreach (Wheel w in vehicle.AllWheels)
        {
            sumRPM += w.RPM;
        }

        DrivetrainRPM = numOfWheels != 0 ? sumRPM / numOfWheels : 0;

        //if the wheels are spinning backwards the car is in "reverse" or is rolling backwards.
        isInReverse = DrivetrainRPM < 0;
    }
    private void DistributePowerAmongWheels()
    {
        int poweredAxisCount = vehicle.PoweredAxis.Count;

        for (int i = 0; i < poweredAxisCount; i++)
        {
            for (int j = 0; j < vehicle.PoweredAxis[i].wheels.Length; j++)
            {
                vehicle.PoweredAxis[i].wheels[j].SetMotorTorque(input.InReverse ?
                    -(engine.EnginePower / vehicle.PoweredAxis[i].wheels.Length) :
                    engine.EnginePower / vehicle.PoweredAxis[i].wheels.Length);
            }
        }

        //BRAKING
        for (int i = 0; i < vehicle.axes.Length; i++)
        {
            for (int j = 0; j < vehicle.axes[i].wheels.Length; j++)
            {
                vehicle.axes[i].wheels[j].SetBrakeTorque(input.brake * 1000);
            }

            //Handbrake is applied to wheels that don't get any power from the engine.
            if (!vehicle.axes[i].isPowered)
            {
                for (int j = 0; j < vehicle.axes[i].wheels.Length; j++)
                    vehicle.axes[i].wheels[j].SetBrakeTorque(input.handbrake * float.MaxValue); //STOP THE WHEELS
            }
        }
    }
    private void ProcessGearChanges()
    {
        if (!vehicle.IsGrounded())
        {
            return;
        }

        if (IsChangingGear)
        {
            gearChangeTimer += Time.fixedDeltaTime;

            if (gearChangeTimer >= timeToChangeGears)
            {
                gearChangeTimer = 0;
                IsChangingGear = false;
            }

            return;
        }

        int gearCount = powerData.gearRatios.Length - 1;

        //TO-DO : THIS SYSTEM IS AUTOMATIC ONLY FOR NOW, MAYBE SEQUENTIAL MANUAL?

        //IF THE ENGINE IS REVVING TO HIGH and there are more gears to go through.
        if (engine.RPM > powerData.maxRPM && CurrentGear < gearCount)
        {
            currentGear++;
        }

        //IF THE ENGINE IS REVVING TO LOW and there are more gears to go through.
        if (engine.RPM < powerData.minRPM && currentGear > 0)
        {
            currentGear--;
        }
    }
}