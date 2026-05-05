using UnityEngine;

public class ShopInteractable : MonoBehaviour
{
    private bool playerInRange = false;
    private ShopManager shopManager;

    void Start()
    {
        shopManager = FindFirstObjectByType<ShopManager>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shopManager != null)
                shopManager.OpenShop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press E to open shop");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}