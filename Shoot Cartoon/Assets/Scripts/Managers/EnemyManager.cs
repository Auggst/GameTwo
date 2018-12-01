using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *类EnemyManager
 * 控制怪物出生，挂在独立的对象上，一种怪物可以对应多个出生地点，只能对应一个脚本
 * 公共变量enemy决定出生的怪物
 * 公共变量spawnTime决定出生时间和出生间隔时间
 * 公共变量组spawnPoints决定出生地点
 */
public class EnemyManager : MonoBehaviour {
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

    private void Start()
    {
        //spawnTime秒后每隔spawnTime重复调用Spawn方法，
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }
        //随机出生地点
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        //出生
        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
