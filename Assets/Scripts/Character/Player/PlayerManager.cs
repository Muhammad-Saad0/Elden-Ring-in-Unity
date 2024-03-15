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

    protected override void LateUpdate()
    {
        base.LateUpdate();

        //  DO NOT PERFORM ANY CAMERA MOVEMENTS IF WE ARE NOT THE OWNER 
        /*  because we have only one camera in the game and it will move 
        for every character we have in game */
        if (!IsOwner)
            return;

        PlayerCamera.instance.HandleAllCameraMovements();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        //SETTING UP PLAYER MANAGER INSIDE PLAYER CAMERA SCRIPT
        /*  if we are the owner then we set this up otherwise camera will start
         following some other players character in your game */
        if (IsOwner)
        {
            PlayerCamera.instance.playerManager = this;
        }
    }
}