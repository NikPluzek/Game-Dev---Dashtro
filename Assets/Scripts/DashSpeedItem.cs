using Unity.AI.Navigation.Samples;
using Unity.VisualScripting;
using UnityEngine;

public class DashSpeedItem : Item
{

    [UnitHeaderInspectable("Dash Speed Boost")]
    [SerializeField] private float speedIncrease = 10f;

    protected override void ApplyEffect(GameObject player)
    {
        WASDMove movement = player.GetComponent<WASDMove>();

        if (movement != null)
        {
            movement.IncreaseDashSpeed(speedIncrease);
            Debug.Log("dash speed increased");
        }
    }

}
