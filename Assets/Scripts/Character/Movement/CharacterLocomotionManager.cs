using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float groundCheckSphereRadius = 0.3f;
    [SerializeField] private float jumpVelocity = 5f;
    [SerializeField] protected float gravityForce = -20f;

    //  WHEN THE CHARACTER IS GROUNDED IT IS BEING PULLED DOWN WITH THIS VELOCITY
    [SerializeField] protected float groundedYVelocity = -20f;
    [SerializeField] protected float fallStartVelocity = -5f;

    [SerializeField] protected Vector3 yVelocity;
    protected float inAirTimer = 0f;
    protected bool fallingVelocityHasBeenSet = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        CheckGrounded();

        //  DONT RUN THE FALLING LOGIC IF IT IS NOT THE WORLD SCENE
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        if (character.isGrounded)
        {
            //  IF WE ARE GROUNDED AND Y VELOCITY IS NOT POSITIVE (meaning we are not trying to jump).
            if (yVelocity.y <= 0)
            {
                //  RESET THE TIMER BECAUSE WE ARE ACTUALLY GROUNDED
                inAirTimer = 0f;
                //  THERE IS NO FALLING VELOCITY WHEN WE ARE ON THE GROUND
                fallingVelocityHasBeenSet = false;
                //  PULL THE CHARACTER DOWN TO STICK IT TO THE GROUND WHEN ITS ALREADY GROUNDED (gives a better felling).
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if (!character.isJumping && !fallingVelocityHasBeenSet)
            {
                Debug.Log("inside very weird if.");
                fallingVelocityHasBeenSet = true;

                //  WE WILL START THE FALL WITH THIS VELOCITY AND INCREASE IT OVER TIME
                yVelocity.y = fallStartVelocity;
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            character.characterAnimator.SetFloat("inAirTimer", inAirTimer);
            yVelocity.y += gravityForce * Time.deltaTime;
        }

        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void CheckGrounded()
    {
        bool groundedCheck = Physics.CheckSphere(transform.position, groundCheckSphereRadius, groundLayers);
        character.isGrounded = groundedCheck;
        character.characterAnimator.SetBool("isGrounded", groundedCheck);
    }
}
