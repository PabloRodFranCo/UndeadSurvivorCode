using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    public SpawnData[] spawnData;
    public float levelTime;

    int level; 
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.Instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        float gameTime = GameManager.Instance.gameTime;

        if (gameTime < 120f)
            level = 0;
        else if (gameTime < 240f)
            level = 1;
        else
            level = 2;
            
        if (timer > spawnData[level].spawnTime / 2)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0); 

        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]); 
    }
}


[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType; 
    public int health; 
    public float speed; 
}