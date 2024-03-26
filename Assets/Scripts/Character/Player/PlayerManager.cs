using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerAnimationController playerAnimationController;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;

    

    override protected void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    override protected void Update()
    {
        base.Update();

        //DONT PERFORM ANY MOVEMENT LOGIC IF WE ARE NOT THE OWNER
        if (!IsOwner)
            return;
        //HANDLE CHARACTER MOVEMENT
        playerLocomotionManager.HandleAllMovement();
        playerStatsManager.RegenerateStamina();
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

        
        if (IsOwner)
        {
            //  SETTING UP PLAYER MANAGER INSIDE PLAYER CAMERA SCRIPT
            /*  if we are the owner then we set this up otherwise camera will start
                following some other players character in your game */
            PlayerCamera.instance.playerManager = this;

            //  SETTING UP THE PLAYER MANAGER IN INPUT MANAGER
            /*  we make sure the player manager that is being set up in PLAYERINPUTMANAGER script
                is the local player (meaning we are owner of this player) otherwise we will start performing
                actions on someone else's character */
            PlayerInputManager.instance.playerManager = this;
            SaveGameManager.instance.player = this;

            //  CHANGING UI WHEN THERE IS A CHANGE IN STAMINA
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.hudManager.setNewStaminaValue;

            //  SUBSCRIBING TO VALUE CHANGE TO RESET THE DELAY TIMER
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaDelayTimer;

            //  CALCULATING MAX STAMINA
            //  -----ENDURANCE IS HARD CODED THIS SHOULD CHANGE-----
            int maxStamina = playerStatsManager.CalculateMaxStaminaBasedOnEnduranceLevel
                (10);

            //  SETTING UP THE MAX STAMINA WHEN PLAYER SPAWNS
            //  We also set the current stamina to max stamina at the start
            playerNetworkManager.maximumStamina.Value = maxStamina;
            PlayerUIManager.instance.hudManager.setMaxStaminaValue(maxStamina);
            playerNetworkManager.currentStamina.Value = maxStamina;
        }
    }
}