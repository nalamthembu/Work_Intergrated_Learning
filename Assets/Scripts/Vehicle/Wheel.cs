using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
    public WheelCollider WheelCollider { get; private set; }
    [SerializeField] private Transform WheelMeshTransform;
    private float WheelSlip;
    public bool IsGettingPower { get; set; }
    public float MaxSteerAngle { get; set; }

    private void Awake() =>  WheelCollider = GetComponent<WheelCollider>();

    private void FixedUpdate()
    {
        UpdateWheel();

        /*
          if (IsGettingPower)
            AdjustSkid();
        */
    }

    private void UpdateWheel()
    {
        WheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        WheelMeshTransform.SetPositionAndRotation(pos, rot);
        WheelCollider.GetGroundHit(out WheelHit hit);
        WheelSlip = Mathf.Abs(hit.forwardSlip + hit.sidewaysSlip);
    }

    private void AdjustSkid()
    {
        float diff = Mathf.Abs(WheelMeshTransform.eulerAngles.y - MaxSteerAngle);

        print(diff);

        WheelFrictionCurve sideWaysFriction = WheelCollider.sidewaysFriction;
        WheelFrictionCurve fwdFriction = WheelCollider.forwardFriction;

        float defaultSidewaysFric = sideWaysFriction.extremumSlip;
        float defaultForwardFric = fwdFriction.extremumSlip;

        sideWaysFriction.extremumSlip = Mathf.Lerp(defaultSidewaysFric, 4F, diff);
        fwdFriction.extremumSlip = Mathf.Lerp(defaultForwardFric, 4F, diff);

        WheelCollider.sidewaysFriction = sideWaysFriction;
        WheelCollider.forwardFriction = fwdFriction;
    }

    public void SetMotorTorque(float motorTorque)
    {
        WheelCollider.motorTorque = motorTorque * 10F; //shit's just slow if I don't multiply it.
    }

    public void SetBrakeTorque(float brakeTorque)
    {
        WheelCollider.brakeTorque = brakeTorque;
    }

    public void SetWheelMeshTransform(Transform wheelMeshTransform) => WheelMeshTransform = wheelMeshTransform;

    public float GetWheelSlip() => WheelSlip;
}