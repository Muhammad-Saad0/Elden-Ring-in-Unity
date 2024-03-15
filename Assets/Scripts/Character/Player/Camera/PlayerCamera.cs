using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public Camera cameraObject;

    [HideInInspector] public PlayerManager playerManager;
    [SerializeField] private GameObject cameraPivot;

    [Header("Camera Speed Variables")]
    [SerializeField] private float followSmoothTime = 1f;
    [SerializeField] private float leftAndRightCameraRotationSpeed = 220f;
    [SerializeField] private float upAndDownCameraRotationSpeed = 160f;

    [Header("Min and Max Look Angles")]
    [SerializeField] private float minimumVerticalLookAngle = -30f;
    [SerializeField] private float maximumVerticalLookAngle = 60f;

    [Header("Camera Collision Variables")]
    [SerializeField] private float raycastSphereRadius = 0.2f;
    [SerializeField] private LayerMask cameraCollisionLayers;
    private Vector3 cameraObjectPosition;
    private float defaultCameraDistanceFromPlayer;
    private float targetCameraDistanceFromPlayer;

    private Vector3 followTargetDirection;
    private Vector3 followTargetReferenceVelocity;

    private float leftAndRightRotateAmount;
    private float upAndDownLookAngle;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        //  FINDING DISTANCE OF CAMERA FROM PLAYER
        defaultCameraDistanceFromPlayer = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraMovements()
    {
        if(playerManager != null)
        {
            HandleFollowTarget();
            HandleCameraRotation();
            HandleCameraCollisons();
        }
    }

    private void HandleFollowTarget()
    {
        followTargetDirection = Vector3.SmoothDamp(transform.position,
            playerManager.transform.position,
            ref followTargetReferenceVelocity,
            followSmoothTime * Time.deltaTime);

        transform.position = followTargetDirection;
    }

    private void HandleCameraRotation()
    {
        //  ROTATING LEFT AND RIGHT
        leftAndRightRotateAmount = PlayerInputManager.instance.horizontalCameraInput 
            * leftAndRightCameraRotationSpeed * Time.deltaTime;

        transform.Rotate(0f, leftAndRightRotateAmount, 0f);

        //  ROTATING UP AND DOWN
        upAndDownLookAngle = Mathf.Clamp
            (upAndDownLookAngle - PlayerInputManager.instance.verticalCameraInput 
            * upAndDownCameraRotationSpeed * Time.deltaTime, 
            minimumVerticalLookAngle, 
            maximumVerticalLookAngle);

        cameraPivot.transform.localRotation = Quaternion.Euler(upAndDownLookAngle, 0f, 0f);
    }

    private void HandleCameraCollisons()
    {
        targetCameraDistanceFromPlayer = defaultCameraDistanceFromPlayer;
        RaycastHit hitInfo;
        Vector3 raycastDirection = cameraObject.transform.position - cameraPivot.transform.position;
        raycastDirection.Normalize();

        if(Physics.SphereCast
            (cameraPivot.transform.position,
            raycastSphereRadius,
            raycastDirection,
            out hitInfo,
            Mathf.Abs(defaultCameraDistanceFromPlayer),
            cameraCollisionLayers))
        {
            float hitPointDistance = Vector3.Distance(cameraPivot.transform.position, hitInfo.point);

            //  SHOULD BE -VE ON Z AXIS
            targetCameraDistanceFromPlayer = -(hitPointDistance - raycastSphereRadius);
        }  

        if(Mathf.Abs(targetCameraDistanceFromPlayer) < raycastSphereRadius)
        {
            targetCameraDistanceFromPlayer = -raycastSphereRadius;
        }

        cameraObjectPosition = cameraObject.transform.localPosition;

        //  LERPING TOWARDS THE REQUIRED CAMERA POSITION
        cameraObjectPosition.z = Mathf.Lerp
            (cameraObject.transform.localPosition.z,
            targetCameraDistanceFromPlayer,
            0.2f);

        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}
