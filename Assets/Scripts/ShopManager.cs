using TMPro;
using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Shop UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform itemContainer;  
    [SerializeField] private GameObject itemButtonPrefab;

    [Header("Items")]
    [SerializeField] private ShopItemData[] shopItems;

    private bool isOpen = false;
    private bool[] purchased;  

    private WASDMove playerMovement;
    private Health playerHealth;
    private PlayerAttack playerAttack;

    void Start()
    {
        shopPanel.SetActive(false);
        purchased = new bool[shopItems.Length];

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<WASDMove>();
            playerHealth = player.GetComponent<Health>();
            playerAttack = player.GetComponent<PlayerAttack>();
        }

        //build the shop buttons
        BuildShopUI();
    }

    private void BuildShopUI()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            int index = i;  
            ShopItemData item = shopItems[i];

            GameObject buttonObj = Instantiate(itemButtonPrefab, itemContainer);

            //set item name
            TextMeshProUGUI[] texts = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                texts[0].text = item.itemName + " - " + item.price + " coins";
                texts[1].text = item.description;
            }

            //wire up button
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => TryPurchase(index));
        }
    }

    public void OpenShop()
    {
        isOpen = true;
        shopPanel.SetActive(true);
        Time.timeScale = 0f;  //pause while shopping
        RefreshButtons();
    }

    public void CloseShop()
    {
        isOpen = false;
        shopPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void TryPurchase(int index)
    {
        if (purchased[index]) return;

        ShopItemData item = shopItems[index];

        if (!CoinManager.Instance.SpendCoins(item.price))
        {
            Debug.Log("Can't afford " + item.itemName);
            return;
        }

        purchased[index] = true;
        ApplyItem(index);
        RefreshButtons();
    }

    private void ApplyItem(int index)
    {
        switch (index)
        {
            case 0: //heal
                playerHealth?.Heal(50f);
                break;
            case 1: //max Health Up
                playerHealth?.IncreaseMaxHealth(50f);
                break;
            case 2: //damage Up
                playerAttack?.IncreaseDamage(15f);
                break;
            case 3: //range Up
                playerAttack?.IncreaseRange(1.5f);
                break;
        }
        Debug.Log("Purchased: " + shopItems[index].itemName);
    }

    private void RefreshButtons()
    {
        Button[] buttons = itemContainer.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            // Grey out if purchased or can't afford
            bool canBuy = !purchased[i] &&
                          CoinManager.Instance.GetCoins() >= shopItems[i].price;
            buttons[i].interactable = canBuy;
        }
    }
}