using Unity.Netcode;
using Unity.Collections;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager player;

    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>
        ("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void HandleEnduranceChanged(int oldEndurance, int newEndurance)
    {
        float newMaxStamina = player.playerStatsManager.CalculateMaxStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.instance.hudManager.SetMaxStaminaValue(newMaxStamina);
        player.playerNetworkManager.currentStamina.Value = newMaxStamina;
    }

    public void HandleVitalityChanged(int oldVitality, int newVitality)
    {
        float newMaxHealth = player.playerStatsManager.CalculateMaxHealthBasedOnVitalityLevel(newVitality);
        PlayerUIManager.instance.hudManager.SetMaxHealthValue(newMaxHealth);
        player.playerNetworkManager.currentHealth.Value = newMaxHealth;
    }
}
