using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private Image healthBarFill;

    [Header("Dash Bar")]
    [SerializeField] private Image dashBarFill;

    private Health playerHealth;
    private WASDMove playerMovement;

    void Start()
    {
        // Find the player
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
            playerMovement = player.GetComponent<WASDMove>();
        }
        else
        {
            Debug.LogError("HUDController: No Player found!");
        }
    }

    void Update()
    {
        UpdateHealthBar();
        UpdateDashBar();
    }

    private void UpdateHealthBar()
    {
        if (playerHealth == null) return;

        // Fill amount is a value between 0 and 1
        // currentHealth / maxHealth gives us exactly that
        healthBarFill.fillAmount = playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth();
    }

    private void UpdateDashBar()
    {
        if (playerMovement == null) return;

        // IsDashReady() returns true/false - we want a smooth fill
        // We need to expose the cooldown progress from WASDMove
        dashBarFill.fillAmount = playerMovement.GetDashCooldownProgress();
    }
}