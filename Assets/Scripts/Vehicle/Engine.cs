using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] private EngineDefinition definition;

    private Vehicle vehicle;

    private Turbo turbo;
    public void SetDefinition(EngineDefinition engineDef) => definition = engineDef;
    public float EngineRPM { get; private set; }

    public float EngineTorque { get; private set; }

    public float MaxRPM { get; private set; }

    public float MinRPM { get; private set; }

    float resistanceFromDriveChain;

    private void Start() => Init();
    
    public void Init()
    {
        MaxRPM = definition.torqueCurve.keys[definition.torqueCurve.length - 1].time;
        MinRPM = definition.torqueCurve.keys[0].time;
        vehicle = GetComponent<Vehicle>();
        turbo = GetComponent<Turbo>();
    }

    private void FixedUpdate()
    {
        UpdateEngine();
    }

    private void UpdateEngine()
    {
        float gearRatio = vehicle.transmission.GetGearRatio(vehicle.transmission.Gear);
        
        float wheelRPM = vehicle.transmission.DrivelineRPM;

        float dampValue = MinRPM + (Mathf.Abs(wheelRPM) * 3.6f * gearRatio);

        bool isHeldBack = vehicle.input.handbrake > 0 && vehicle.input.throttle > 0 || vehicle.VehicleSpeedKMPH <= 0 && vehicle.input.throttle > 0;

        EngineRPM = Mathf.Clamp(MathLib.Damp(EngineRPM,  isHeldBack ? MaxRPM * vehicle.input.throttle : dampValue, 5F, Time.fixedDeltaTime), MinRPM, MaxRPM);

        resistanceFromDriveChain = EngineRPM - (EngineRPM - wheelRPM);

        EngineTorque = definition.torqueCurve.Evaluate(EngineRPM - resistanceFromDriveChain);
    }
}
