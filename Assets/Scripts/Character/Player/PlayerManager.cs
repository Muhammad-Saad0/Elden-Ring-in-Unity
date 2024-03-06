using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerLocomotionManager playerLocomotionManager;

    override protected void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    override protected void Start()
    {
        base.Start();

        //HANDLE CHARACTER MOVEMENT
        playerLocomotionManager.HandleAllMovement();
    }
}