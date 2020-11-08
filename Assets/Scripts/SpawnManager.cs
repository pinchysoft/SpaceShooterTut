using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _waitTime = 5f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerContainer;
    [SerializeField]
    private GameObject _collectableContainer;
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject[] _Collectables;

    private bool _stopSpawning = false;
    private bool _stopSpawningAmmoHealth = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
        StartCoroutine(SpawnCollectables());
    }

    private IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            float randomX = Random.Range(-8f, 8f);
            Vector3 location = new Vector3(randomX, 7f, 0);
            int list = Random.Range(0, 4);
            GameObject newPowerUp = Instantiate(_powerUps[list], location, Quaternion.identity);
            newPowerUp.transform.parent = _powerContainer.transform;
        }
    }
    private IEnumerator SpawnCollectables()
    {
        yield return new WaitForSeconds(5f);
        while (!_stopSpawningAmmoHealth)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            float randomX = Random.Range(-8f, 8f);
            Vector3 location = new Vector3(randomX, 7f, 0);
            int list = Random.Range(0, 2);
            GameObject newCollectable = Instantiate(_Collectables[list], location, Quaternion.identity);
            newCollectable.transform.parent = _collectableContainer.transform;
        }
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 location = new Vector3(randomX, 7f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, location, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_waitTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        _stopSpawningAmmoHealth = true;
    }
}
