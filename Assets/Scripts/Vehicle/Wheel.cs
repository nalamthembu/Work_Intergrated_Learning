using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
    new WheelCollider collider;
    [SerializeField] GameObject wheelMeshPrefab;
    [Tooltip("Make sure the mesh has a parent it can rotate on")]
    [SerializeField] Transform wheelMeshParent;
    [SerializeField] WheelPosition wheelPosition;
    [SerializeField] WheelOutfacingDirection wheelSide;

    //"r = Realtime, or Item that is spawned in during gameplay
    private WheelSlip slip;
    private GameObject rWheel;
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

        InitialiseWheelMesh();

        ResizeWheelCollider();
    }

    public void SetMotorTorque(float torque) => collider.motorTorque = torque;
    public void SetBrakeTorque(float bTorque) => collider.brakeTorque = bTorque;

    private void FixedUpdate()
    {
        collider.GetGroundHit(out WheelHit hit);
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        rWheel.transform.parent.SetPositionAndRotation(pos, rot);
        slip.forward = hit.forwardSlip;
        slip.sideways = hit.sidewaysSlip;
    }

    private void ResizeWheelCollider()
    {
        Bounds b = rWheel.GetComponent<MeshRenderer>().bounds;
        collider.radius = b.size.z / 2;
    }

    private void InitialiseWheelMesh()
    {
        rWheel = Instantiate(wheelMeshPrefab, wheelMeshParent.position, wheelMeshParent.rotation, wheelMeshParent);

        switch (wheelSide)
        {
            case WheelOutfacingDirection.LEFT:
                rWheel.transform.localEulerAngles = Vector3.up * -180;
                break;
        }
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