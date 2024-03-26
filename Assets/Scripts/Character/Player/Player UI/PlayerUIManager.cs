using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [HideInInspector] public PlayerUIHudManager hudManager;

    [Tooltip("Network Setting")]
    [SerializeField] private bool startAsClient = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        hudManager = GetComponentInChildren<PlayerUIHudManager>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    private void Update()
    {
        if (startAsClient)
        {
            startAsClient = false;

            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }
    }
}
