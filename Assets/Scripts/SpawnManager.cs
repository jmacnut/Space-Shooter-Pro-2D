using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _waitTime = 5.0f;
    [SerializeField]
    private bool _stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // spawns game objects every 5 seconds
    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_waitTime);   // delay next event
        }

        // infinite loop; will never get here!
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
