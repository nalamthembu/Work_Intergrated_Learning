using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 5F;
    [SerializeField] Transform target;
    [SerializeField] Vector2 pitchMinMax = new(-40, 85);
    [SerializeField] Vector2 cameraOffset = new(.5F, 0);
    [SerializeField] float distanceFromTarget = 2F;

    public static CameraController instance;

    float pitch;
    float yaw;
    public float PreviousDistance { get; private set; }
    public float PreviousFOV { get; private set; }
    float targetFOV; 

    public float[] PitchYaw
    {
        get
        {
            return new float[]
            {
                pitch,
                yaw
            };
        }
    }

    public float MouseSensitivity { get { return mouseSensitivity; } }

    PlayerInput input;
    CharacterSituationalStateMachine playerSitMachine;
    new Camera camera;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        input = FindObjectOfType<PlayerInput>();
        playerSitMachine = FindObjectOfType<PlayerCharacter>().GetComponent<CharacterSituationalStateMachine>();
        camera = GetComponentInChildren<Camera>();

        if (!camera)
            Debug.LogError("Make sure there is a camera as a child object");

        PreviousDistance = distanceFromTarget;
        PreviousFOV = camera.fieldOfView;
        targetFOV = PreviousFOV;
    }

    private void Update() => HandleInput();

    private void LateUpdate()
    { 
        Vector3 targetRotation = new(pitch, yaw);
        transform.eulerAngles = targetRotation;
        camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, cameraOffset, Time.deltaTime);
        transform.position = target.position - transform.forward * distanceFromTarget;
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFOV, Time.deltaTime * 2F);
    }

    private void HandleInput()
    {
        yaw += input.GetMouseX(mouseSensitivity);
        pitch -= input.GetMouseY(mouseSensitivity);
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
    }

    public void SetTarget(Transform target, float distance, float fov)
    {
        this.target = target;
        distanceFromTarget = distance;
        targetFOV = fov;
    }
}