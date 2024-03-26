using Unity.Netcode;
using UnityEngine;

public class CharacterNetworkManager : NetworkBehaviour
{
    CharacterManager character;

    [Header("Network Movement Variables")]
    [HideInInspector] public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>
        (Vector3.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [HideInInspector] public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>
        (Quaternion.identity,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [HideInInspector] public Vector3 networkReferenceVelocity;
    [HideInInspector] public float networkSmoothTime = 0.1f;
    [HideInInspector] public float networkRotationSmoothTime = 0.1f;

    [Header("Network Animator Variables")]
    [HideInInspector] public NetworkVariable<float> animatorHorizontalValue = new NetworkVariable<float>
        (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [HideInInspector] public NetworkVariable<float> animatorVerticalValue = new NetworkVariable<float>
        (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Flags")]
    [HideInInspector] public NetworkVariable<bool> sprintingValue = new NetworkVariable<bool>
        (false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Stats")]
    //  STAMINA VARIABLES
    [HideInInspector] public NetworkVariable<int> endurance = new NetworkVariable<int>
        (10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [HideInInspector] public NetworkVariable<float> currentStamina = new NetworkVariable<float>
        (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [HideInInspector] public NetworkVariable<float> maximumStamina = new NetworkVariable<float>
        (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }


    #region Rpcs(Remote Procedure Calls)
    [ServerRpc]
    public void NotifyServerOfPlayerActionAnimationServerRpc
        (ulong cliendID,
        string targetAnimation,
        bool applyRootMotion)
    {
        /*  IT IS NOT NECESSARY TO CHECK IF ITS THE SERVER BECAUSE SERVERRpc WILL EXECUTE ON THE SERVER
            but we check it anyways */
        if (IsServer)
        {
            PlayPlayerActionAnimationOnAllClientsClientRpc(cliendID, targetAnimation, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayPlayerActionAnimationOnAllClientsClientRpc
        (ulong cliendID,
        string targetAnimation,
        bool applyRootMotion)
    {
        if(IsClient && cliendID != NetworkManager.LocalClientId)
        {
            PerformActionAnimationFromServer(targetAnimation, applyRootMotion);
        }
    }
    #endregion

    private void PerformActionAnimationFromServer(string targetAnimation, bool applyRootMotion)
    {
        character.applyRootMotion = applyRootMotion;
        character.characterAnimator.CrossFade(targetAnimation, 0.2f);
    }
}
