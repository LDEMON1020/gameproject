using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefabs;
    public GameObject RocketPrefabs;

    [Header("���� Ÿ�̹� ����")]
    public float minSpawninternal = 0.5f;
    public float maxSpawninternal = 2.0f;

    [Header("���� Ÿ�̹� ����")]
    [Range(0, 100)]
    public int coinSpawnchance = 50;

    public float timer = 0.0f;
    public float nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        SetNextSpawnTime();
      
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawnTime)
        {
            SpawnObject();
            timer = 0.0f;
            SetNextSpawnTime();
        }
    }
void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawninternal, maxSpawninternal);
    }

    void SpawnObject()
    {
        Transform spawnTransform = transform;

        int randomValue = Random.Range(0, 100);
        if(randomValue < coinSpawnchance)
        {
            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Instantiate(RocketPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
            
    }

}


