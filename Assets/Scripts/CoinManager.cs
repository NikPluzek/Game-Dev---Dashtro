using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinText;

    private int coins = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (coins < amount)
            return false; // Can't afford it

        coins -= amount;
        UpdateUI();
        return true;
    }

    public int GetCoins() => coins;

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = ""+ coins;
    }
}