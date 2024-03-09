using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterController playerController;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        playerController = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
    }
}
