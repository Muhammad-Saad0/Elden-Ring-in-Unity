using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerManager : CharacterManager
{
    PlayerLocomotionManager playerLocomotionManager;

    override protected void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    override protected void Update()
    {
        base.Update();

        //DONT PERFORM ANY MOVEMENT LOGIC IF WE ARE NOT THE OWNER
        if (!IsOwner)
            return;
        //HANDLE CHARACTER MOVEMENT
        playerLocomotionManager.HandleAllMovement();
    }
}