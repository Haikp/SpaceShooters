using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _playerIsAlive = true;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    public GameObject[] powerupArr;
    void Start()
    {

    }

    public void BeginSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (_playerIsAlive)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-10f, 10f), 7, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (true)
        {
            Instantiate(powerupArr[Random.Range(0, powerupArr.Length)], new Vector3(Random.Range(-10f, 10f), 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void OnPlayerDeath()
    {
        _playerIsAlive = false;
    }
}
