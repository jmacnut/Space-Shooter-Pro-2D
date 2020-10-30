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

    //[Header("Triple Shot Powerup Parameters")]
    //[SerializeField]
    //private GameObject _TripleShotPowerupPrefab;
    private float _waitTimePowerup = 7.0f;
    [SerializeField]

    [Header("General Parameters")]
    private bool _stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
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
        while (_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0f);
            int randomPowerupIndex = Random.Range(0, 3); // extend to 3 when shields collection is implemented
            GameObject newPowerup = Instantiate(_powerupPrefabs[randomPowerupIndex], spawnPos, Quaternion.identity);
            _waitTimePowerup = Random.Range(3, 8);   // 3 to 7 seconds
            yield return new WaitForSeconds(_waitTimePowerup);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
