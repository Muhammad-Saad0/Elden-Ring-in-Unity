using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterNetworkManager : NetworkBehaviour
{
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
}
