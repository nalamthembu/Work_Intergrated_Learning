using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
    [SerializeField] new WheelCollider collider;
    [SerializeField] Transform wheelMesh;
    [SerializeField] WheelPosition wheelPosition;
    [SerializeField] WheelOutfacingDirection wheelSide;

    //"r = Realtime, or Item that is spawned in during gameplay
    private WheelSlip slip;
    private GameObject rTyre;

    private void Awake()
    {
        collider = GetComponent<WheelCollider>();
        rTyre = wheelMesh.gameObject;
        ResizeWheelCollider();
    }

    public void FixedUpdate()
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
    FRONT
}
#endregion