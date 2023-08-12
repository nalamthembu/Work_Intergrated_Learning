using UnityEngine;

public class VehicleEngine : MonoBehaviour
{
    private VehicleTransmission transmission;

    private VehicleInput input;

    private float totalEnginePower;

    private float engineRPM;

    private const float IDLERPM = 1000;

    [SerializeField][Range(0.5F, 10F)] float rpmSmoothTime;

    public float EnginePower { get { return totalEnginePower; } }

    public float RPM { get { return engineRPM; } }

    private void Awake()
    {
        transmission = GetComponent<VehicleTransmission>();
        input = GetComponent<VehicleInput>();
    }

    private void FixedUpdate()
    {
        CalculateEnginePower();
    }

    private void CalculateEnginePower()
    {
        //TO-DO : ADD A REV LIMITER

        float gearRatio = transmission.powerData.gearRatios[transmission.CurrentGear];

        totalEnginePower = (transmission.powerData.torqueCurve.Evaluate(engineRPM) * gearRatio) * input.throttle;

        float velocity = 0;

        engineRPM = Mathf.SmoothDamp(engineRPM, IDLERPM + (Mathf.Abs(transmission.DrivetrainRPM * 3.6F * gearRatio)), ref velocity, rpmSmoothTime);
    }
}