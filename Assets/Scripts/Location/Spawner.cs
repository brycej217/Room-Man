using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private GameObject enemy;
    public List<GameObject> currentEnemies = new List<GameObject>();

    [Header("Params")]
    [SerializeField]
    private float spawnRate = 0.1f;
    [SerializeField]
    private int numWaves = 5;
    [SerializeField]
    private int numPerWave = 5;
    [SerializeField]
    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(FetchPlayer());
        Enemy.OnDeath += OnDeathHandler;
    }

    private IEnumerator FetchPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (Player.Instance == null)
        {
            yield return wait;
        }

        player = Player.Instance;
    }

    public void StartSpawn()
    {
        if (player)
        {
            SpawnWave();
            return;
        }
        Debug.LogWarning("Spawning failed player not yet instantiated");
    }

    private void SpawnWave()
    {
        currentWave += 1;
        for (int i = 0; i < numPerWave; i++)
        {
            StartCoroutine(SpawnCoroutine());
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f / spawnRate);

        for (int i = 0; i < numPerWave; i++)
        {
            currentEnemies.Add(SpawnEnemy());
            yield return wait;
        }
    }

    private GameObject SpawnEnemy()
    {
        return Instantiate(enemy, transform, false);
    }

    private void ResetWave()
    {
        if (currentWave < numWaves)
        {
            SpawnWave();
        }
        else
        {
            Debug.LogWarning("Victory");
        }
    }

    private void OnDeathHandler(Transform enemy)
    {
        currentEnemies.Remove(enemy.gameObject);
    }

}