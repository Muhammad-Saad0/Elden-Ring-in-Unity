using Unity.Netcode;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    private static TitleScreenManager instance;

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
    public void ConnectToNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        StartCoroutine(SaveGameManager.instance.LoadNewGame());
    }
}
