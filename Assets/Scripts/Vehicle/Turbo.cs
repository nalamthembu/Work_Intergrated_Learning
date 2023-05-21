using UnityEngine;

[RequireComponent(typeof(Vehicle))]
public class Turbo : MonoBehaviour
{
    [SerializeField] TurboDefinition definition;
    private Engine engine;
    private VehicleInput input;
    private Transmission trans;

    public bool TurboActive { get; private set; }
    public float CurrentBoost { get; private set; }
    public float TurboRPM { get; private set; }

    private void Awake()
    {
        engine = GetComponent<Engine>();
        input = GetComponent<VehicleInput>();
        trans = GetComponent<Transmission>();
    }

    private void FixedUpdate() => ProcessTurbo();

    private void ProcessTurbo()
    {
        if (engine.EngineRPM >= engine.EngineRPM * trans.GEAR_CHANGE_RPM_PERCENTAGE / 2 && input.throttle > 0.45F)
            TurboRPM += 1000 * Time.fixedDeltaTime;
        else
            if (engine.EngineRPM < engine.EngineRPM * trans.GEAR_CHANGE_RPM_PERCENTAGE / 4 && input.throttle <= 0.45F)
        {
            print("anti_lag");
            TurboRPM += 5000 * Time.fixedDeltaTime;
        }
        else
            TurboRPM -= 500 * Time.fixedDeltaTime;

        TurboRPM = Mathf.Clamp(TurboRPM, 0, 10000F);
    }
}