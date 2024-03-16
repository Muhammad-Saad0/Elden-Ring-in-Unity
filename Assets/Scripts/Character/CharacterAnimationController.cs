using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    CharacterManager character;
    [SerializeField] private float animationDamptime = 0.1f;

    private float verticalMovement;
    private float horizontalMovement;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateCharacterAnimatorParameters(float horizontalValue, float verticalValue)
    {
        verticalMovement = verticalValue;
        horizontalMovement = horizontalValue;

        if (character.characterNetworkManager.sprintingValue.Value)
        {
            verticalMovement = 2f;
        }

        //CHANGING ANIMATION PARAMETERS
        character.characterAnimator.SetFloat("Horizontal", horizontalMovement, animationDamptime, Time.deltaTime);
        character.characterAnimator.SetFloat("Vertical", verticalMovement, animationDamptime, Time.deltaTime);
    }

    //  FOR SOME ANIMATIONS WE NEED THE ROOT MOTION OF ANIMATIONS LIKE ROLLING ETC SO WE HAVE A PARAMETER FOR THAT
    public virtual void PlayTargetAnimation
        (string targetAnimation,
        bool applyRootMotion,
        bool canMove = false,
        bool canRotate = false,
        bool isPerformingAction = true)
    {
        character.applyRootMotion = applyRootMotion;
        //  SECOND PARAMETER IS THE TRANSITION DURATION.
        character.characterAnimator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;
    }
}
