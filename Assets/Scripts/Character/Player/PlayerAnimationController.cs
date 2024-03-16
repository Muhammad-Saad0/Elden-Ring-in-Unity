using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : CharacterAnimationController
{
    PlayerManager playerManager;
    override protected void Awake()
    {
        base.Awake();

        playerManager = GetComponent<PlayerManager>();
    }

    /*  This function gets called every frame the animator is playing animation 
        this function will be called even when walking animation is playing */
    private void OnAnimatorMove()
    {
        if(playerManager.applyRootMotion)
        {
            //  MANUALLY APPLYING THE VELOCITY AND ROTAION OF ROOT MOTION
            Vector3 rootMotionVelocity = playerManager.characterAnimator.deltaPosition;
            playerManager.characterController.Move(rootMotionVelocity);
            playerManager.transform.rotation *= playerManager.characterAnimator.deltaRotation;
        }
    }
}
