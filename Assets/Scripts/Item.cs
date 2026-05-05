using Unity.VisualScripting;
using UnityEngine;

public abstract class Item : MonoBehaviour
{

    [UnitHeaderInspectable("Item Settings")]
    [SerializeField] private string itemName = "item";

    private bool hasBeenPickedUp = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Item trigger hit by: " + other.gameObject.name);
        if (other.CompareTag("Player") && !hasBeenPickedUp)
        {

            hasBeenPickedUp = true;
            ApplyEffect(other.gameObject);
            Debug.Log("picked up " + itemName);
            Destroy(gameObject);
        }
    }

    protected abstract void ApplyEffect(GameObject player);
}
