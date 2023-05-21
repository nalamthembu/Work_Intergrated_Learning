using UnityEngine;

public class Transmission : MonoBehaviour
{
    [SerializeField] TransmissionDefinition definition;

    [SerializeField] TransmissionType transmissionType;

    private Vehicle vehicle;
    public float DrivelineRPM { get; private set; }
    public int Gear { get; private set; }

    private int poweredWheelCount;

    public bool currentlyChangingGears;

    [HideInInspector]
    public float GEAR_CHANGE_RPM_PERCENTAGE = 0.25F; 

    public void SetDefinition(TransmissionDefinition transDef) => definition = transDef;

    private void Start() => Init();

    private void FixedUpdate()
    {
        CalculateOptimalGear();
        TransferPowerToWheels();
    }

    public void Init()
    {
        vehicle = GetComponent<Vehicle>();

        for (int i = 0; i < vehicle.axes.Length; i++)
        {
            for (int j = 0; j < vehicle.axes[i].wheels.Length; j++)
            {
                Wheel w = vehicle.axes[i].wheels[j];

                w.MaxSteerAngle = Mathf.Rad2Deg *
                            Mathf.Atan(2.55F / (vehicle.axes[i].steerRadius + (1.5f / 2)))
                            * 1;

                if (vehicle.axes[i].powered)
                {
                    w.IsGettingPower = true;
                    vehicle.axes[i].wheels[j] = w;
                }
            }
        }
    }

    private void TransferPowerToWheels()
    {
        DrivelineRPM = 0;

        for (int i = 0; i < vehicle.axes.Length; i++)
        {
            for (int j = 0; j < vehicle.axes[i].wheels.Length; j++)
            {
                Wheel w = vehicle.axes[i].wheels[j];

                if (vehicle.axes[i].powered)
                {
                    if (poweredWheelCount != vehicle.axes[i].WheelCount)
                        poweredWheelCount = vehicle.axes[i].WheelCount;

                    DrivelineRPM += w.WheelCollider.rpm;
                }
            }
        }

        if (poweredWheelCount <= 0)
            return;

        DrivelineRPM = Mathf.Abs(DrivelineRPM) / poweredWheelCount;

        for (int i = 0; i < vehicle.axes.Length; i++)
        {
            for (int j = 0; j < vehicle.axes[i].wheels.Length; j++)
            {
                Wheel wheel = vehicle.axes[i].wheels[j];

                if (vehicle.axes[i].powered && !currentlyChangingGears) //don't transfer power to wheels while changing gears.
                {
                    float input = (vehicle.input.reverse) ? -vehicle.input.throttle : vehicle.input.throttle;

                    input = (vehicle.VehicleSpeedKMPH <= 0) ? 1 * vehicle.input.clutch : input;

                    float power = vehicle.engine.EngineTorque / poweredWheelCount * input;

                    float bitePoint = .5F;

                    if (vehicle.input.clutch < bitePoint)
                    {
                        float coastPower = vehicle.localVelocity.magnitude;
                        wheel.SetMotorTorque(coastPower);
                        continue;
                    }

                    wheel.SetMotorTorque(power);
                }
            }
        }
    }

    public float GetGearRatio(int gear)
    {
        if (vehicle.input.reverse)
            return definition.reverseGearRatio;

        return definition.gearRatios[gear];
    }

    float currentGearChangeDuration;

    private void CalculateOptimalGear()
    {

        int gearCount = definition.gearRatios.Length - 1;

        float slipThreshold = 1.5F;

        switch (transmissionType)
        {
            case TransmissionType.AUTO:

                if (vehicle.input.reverse)
                {
                    //QUICK EASY SOLUTION
                    if (vehicle.input is null)
                        Init();

                    currentlyChangingGears = false;
                    break;
                }

                if (!vehicle.Grounded)
                    break;

                currentlyChangingGears = currentGearChangeDuration != 0;

                if (vehicle.VehicleSpeedKMPH <= 0)
                {
                    currentGearChangeDuration = 0;
                    currentlyChangingGears = false;
                    ForceIntoGear(0);
                }

                if (vehicle.GetAxesWheelSlip() < slipThreshold)
                {
                    bool isAtOptimalSpeed = vehicle.VehicleSpeedKMPH >= definition.optimalSpeed[Gear];

                    if (isAtOptimalSpeed && Gear < gearCount)
                    {
                        bool rpmIsHighEnoughToChange = vehicle.engine.EngineRPM >= vehicle.engine.MaxRPM * GEAR_CHANGE_RPM_PERCENTAGE;

                        if (rpmIsHighEnoughToChange)
                        {
                            if (currentGearChangeDuration >= definition.gearChangeDuration)
                            {
                               // vehicle.input.clutch = 0;
                                ShiftIntoOptimalGearForSpeed(vehicle.VehicleSpeedKMPH);
                                currentGearChangeDuration = 0;
                            }
                            else
                                currentGearChangeDuration += Time.deltaTime;
                        }
                    }
                }

                bool isNotAtOptimalSpeed = vehicle.VehicleSpeedKMPH < definition.optimalSpeed[Gear];

                if (isNotAtOptimalSpeed)
                {
                    bool rpmIsGoodEnough = vehicle.engine.EngineRPM <= vehicle.engine.MaxRPM * GEAR_CHANGE_RPM_PERCENTAGE - 0.05F;

                    if (rpmIsGoodEnough)
                    {
                        if (currentGearChangeDuration >= definition.gearChangeDuration)
                        {
                            ShiftIntoOptimalGearForSpeed(vehicle.VehicleSpeedKMPH);
                            currentGearChangeDuration = 0;
                        }
                        else
                            currentGearChangeDuration += Time.deltaTime;
                    }
                }

                break;

            case TransmissionType.MANUAL:
                break;

        }
    }

    private void ShiftIntoOptimalGearForSpeed(float speed)
    {
        for (int i = 0; i < definition.optimalSpeed.Length; i++)
        {
            if (definition.optimalSpeed[i] >= Mathf.Floor(speed))
            {
                Gear = i;
               // vehicle.input.clutch = 1F;
                break;
            }
        }
    }

    private void ForceIntoGear(int gear) => Gear = gear;
    
}

public enum TransmissionType
{
    AUTO,
    MANUAL
}