using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    [Header("Room Settings")]
    public RoomType roomType;
    public Transform playerSpawnPoint; // Where player appears when entering this room

    [Header("Doors")]
    public Door[] doors; // All doors in this room

    [Header("Camera")]
public Transform cameraTarget;

    private int enemyCount = 0;
    private bool isCleared = false;
    private bool hasBeenEntered = false;

    public enum RoomType
    {
        Combat,
        Treasure,
        Boss,
        Start
    }

    void Start()
    {

        // Find all enemies in this room
        Health[] foundEnemies = GetComponentsInChildren<Health>();
        Debug.Log(gameObject.name + " found " + foundEnemies.Length + " enemies");

        foreach (Health enemy in foundEnemies)
        {
            if (enemy.gameObject.tag != "Player")
            {
                enemyCount++;
                enemy.onDeath.AddListener(OnEnemyDied);
                Debug.Log("Subscribed to: " + enemy.gameObject.name);

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null) agent.enabled = false;
            }
        }
    }

    public void OnRoomEntered()
    {
        BossEnemy boss = GetComponentInChildren<BossEnemy>();
        if (boss != null)
            boss.Activate();

        if (hasBeenEntered) return;
        hasBeenEntered = true;

        Health[] foundEnemies = GetComponentsInChildren<Health>();
        foreach (Health enemy in foundEnemies)
        {
            if (enemy.gameObject.tag != "Player")
            {
                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null) agent.enabled = true;
            }
        }

        // Lock doors if it's a combat or boss room with enemies
        if ((roomType == RoomType.Combat || roomType == RoomType.Boss) && enemyCount > 0)
        {
            SetDoorsLocked(true);
        }

        BossHealthBar bossHealthBar = FindFirstObjectByType<BossHealthBar>();

        if (boss != null && bossHealthBar != null)
        {
            Health bossHealth = boss.GetComponent<Health>();
            if (bossHealth != null)
                bossHealthBar.ShowBossBar(bossHealth);
        }
    }

    private void OnEnemyDied()
    {
        enemyCount--;
        Debug.Log("Enemy died! Remaining: " + enemyCount);

        if (enemyCount <= 0 && !isCleared)
        {
            ClearRoom();
        }
    }

    private void ClearRoom()
    {
        isCleared = true;
        Debug.Log(gameObject.name + " cleared!");
        SetDoorsLocked(false);
    }

    private void SetDoorsLocked(bool locked)
    {
        foreach (Door door in doors)
        {
            if (door != null)
                door.SetLocked(locked);
        }
    }

    public bool IsCleared() => isCleared;
}