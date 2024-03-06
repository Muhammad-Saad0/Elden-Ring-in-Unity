using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Tooltip("Network Setting")]
    [SerializeField]
    private bool startAsClient = false;

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
