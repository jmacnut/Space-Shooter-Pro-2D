using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Spawn Parameters")]
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _waitTimeEnemy = 5.0f;

    [Header("Powerup Parameters")]
    [SerializeField]
    private GameObject[] _powerupPrefabs;
    [SerializeField]
    private float _waitTimePowerup = 7.0f;

    [Header("Collectable Parameters")]
    [SerializeField]
    private GameObject[] _collectablePrefabs;
    private float _waitTimeCollectable = 7.0f;

    [Header("General Parameters")]
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnCollectableRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_waitTimeEnemy);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0f);

            // Framework: Octo Shot Powerup weighted rare, 10%
            int[] weightedIndex = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3 };
            int randomIndex = Random.Range(0, 10);
            int randomPowerupIndex = weightedIndex[randomIndex];
            //int randomPowerupIndex = Random.Range(0, 4); // 3 s/b rare, extend to 5 for additional powerups collected

            GameObject newPowerup = Instantiate(_powerupPrefabs[randomPowerupIndex], spawnPos, Quaternion.identity);
            _waitTimePowerup = Random.Range(3, 8);   // 3 to 7 seconds
            yield return new WaitForSeconds(_waitTimePowerup);
        }
    }

    IEnumerator SpawnCollectableRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0f);
            int randomPowerupIndex = Random.Range(0, 2);   // increase for additional collectables
            GameObject newPowerup = Instantiate(_collectablePrefabs[randomPowerupIndex], spawnPos, Quaternion.identity);
            _waitTimeCollectable = Random.Range(3, 8);   // 3 to 7 seconds
            yield return new WaitForSeconds(_waitTimeCollectable);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
