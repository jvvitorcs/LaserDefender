using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs, waveBoss, spawnBossList;
    [SerializeField] int startingWave = 0, waveCounter = 0;
    
    public bool enemies = false, bosswave = false;

    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (enemies);

        do
        {
            yield return StartCoroutine(SpawnBossWaves());
        } while (bosswave);
    }

    void Update()
    {
        WaveCounts();
    }

    void WaveCounts()
    {
        if (waveCounter == 2)
        {
            enemies = false;
            bosswave = true;
        }
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
        waveCounter++;
    }
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWayPoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }


    //Bosswave
    private IEnumerator SpawnBossWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveBoss.Count; waveIndex++)
        {
            var currentWave = waveBoss[waveIndex];
            yield return StartCoroutine(SpawnBossEnemiesInWave(currentWave));
        }
        StartCoroutine(SpawnBoss());
    }
    private IEnumerator SpawnBossEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWayPoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
        bosswave = false;
    }

    //Boss
    private IEnumerator SpawnBoss()
    {
        for (int waveIndex = startingWave; waveIndex < spawnBossList.Count; waveIndex++)
        {
            var currentWave = spawnBossList[waveIndex];
            yield return StartCoroutine(SpawnBossHere(currentWave));
        }
        waveCounter++;
    }

    private IEnumerator SpawnBossHere(WaveConfig waveConfig)
    {
        for (int bossCount = 0; bossCount < 1; bossCount++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWayPoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            StopCoroutine(SpawnBoss());
            yield return null;
        }
    }
}
