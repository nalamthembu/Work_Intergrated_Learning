using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
    new WheelCollider collider;
    [Tooltip("Make sure the mesh has a parent it can rotate on")]
    [SerializeField] Transform wheelMesh;
    [SerializeField] WheelPosition wheelPosition;
    [SerializeField] WheelOutfacingDirection wheelSide;

    //"r = Realtime, or Item that is spawned in during gameplay
    private WheelSlip slip;
    private GameObject rTyre;

    public bool IsGrounded
    {
        get
        {
            return collider.isGrounded;
        }
    }

    public float SteeringAngle
    {
        get
        {
            return collider.steerAngle;
        }

        set
        {
            collider.steerAngle = value;
        }
    }

    public float RPM { get { return collider.rpm; } }


    private void Awake()
    {
        collider = GetComponent<WheelCollider>();
        rTyre = wheelMesh.gameObject;
        ResizeWheelCollider();
    }

    public void SetMotorTorque(float torque) => collider.motorTorque = torque;
    public void SetBrakeTorque(float bTorque) => collider.brakeTorque = bTorque;

    private void FixedUpdate()
    {
        collider.GetGroundHit(out WheelHit hit);
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        rTyre.transform.SetPositionAndRotation(pos, rot);
        slip.forward = hit.forwardSlip;
        slip.sideways = hit.sidewaysSlip;
    }

    private void ResizeWheelCollider()
    {
        Bounds b = rTyre.GetComponent<MeshRenderer>().bounds;
        collider.radius = b.size.z / 2;
    }
}

#region STRUCTS
public struct WheelSlip
{
    public float forward;
    public float sideways;
}

public enum WheelOutfacingDirection
{
    LEFT,
    RIGHT
}

public enum WheelPosition
{
    BACK,
    MID, //For lorries
    FRONT
}
#endregion