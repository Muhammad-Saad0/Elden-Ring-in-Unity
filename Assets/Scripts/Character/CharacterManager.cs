using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public CharacterNetworkManager characterNetworkManager;
    [HideInInspector] public Animator characterAnimator;
    [HideInInspector] public CharacterAnimationController characterAnimationController;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canMove = true;
    public bool canRotate = true;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterAnimator = GetComponent<Animator>();
        characterAnimationController = GetComponent<CharacterAnimationController>();
    }

    protected virtual void Update()
    {
        if (IsOwner)
        {
            //  IF WE ARE THE OWNER UPDATE THE NETWORK VARIABLES
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;

            //  SETTING ANIMATION NETWORK VARIABLES
            characterNetworkManager.animatorHorizontalValue.Value = characterAnimator.GetFloat("Horizontal");
            characterNetworkManager.animatorVerticalValue.Value = characterAnimator.GetFloat("Vertical");
        }
        else
        {
            //IF NOT THE OWNER UPDATE THE POSITION AND ROTATION 
            transform.position = Vector3.SmoothDamp
                (transform.position,
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkReferenceVelocity,
                characterNetworkManager.networkSmoothTime);

            transform.rotation = Quaternion.Slerp
                (transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);

            characterAnimationController.UpdateCharacterAnimatorParameters
                (characterNetworkManager.animatorHorizontalValue.Value,
                characterNetworkManager.animatorVerticalValue.Value);
        }
    }

    protected virtual void LateUpdate()
    {
        
    }
}
