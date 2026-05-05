using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private GameObject bossBarRoot;
    [SerializeField] private Image bossBarFill;

    private Health bossHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (bossBarRoot != null)
        {
            bossBarRoot.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth == null) return;

        bossBarFill.fillAmount = bossHealth.GetCurrentHealth() / bossHealth.GetMaxHealth();

    }

    public void ShowBossBar(Health boss)
    {
        bossHealth = boss;

        bossHealth.onDeath.AddListener(OnBossDied);

        if (bossBarRoot != null)
        {
            bossBarRoot.SetActive(true);
        }
    }

    private void OnBossDied()
    {
        bossBarFill.fillAmount = 0f;
        Invoke("HideBar", 0.5f);
    }

    private void HideBar()
    {
        if (bossBarRoot != null)
            bossBarRoot.SetActive(false);
    }
}
