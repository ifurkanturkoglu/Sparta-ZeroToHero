﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
public class GenerateEnemy : MonoBehaviour
{
    public static GenerateEnemy Instance;
    int totalEnemyCount;
    public int faster, ranger, tank = 0;
    public int enemyCount;
    public GameObject[] prefabs;
    [SerializeField] Transform enemies, player;
    GameObject cloneEnemy;
    int lowerBound = 3, upperBound = 3;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && GameManager.Instance.waveComplete && !Elevator.Instance.elevatorIsRun && !Elevator.Instance.elevatorLocation)
        {
            UIManager.Instance.InformationTextUpdate("", Color.green);
            GameManager.Instance.waveComplete = false;
            GameManager.Instance.IncreaseWave();
            NewEnemy();
        }
    }
    private void NewEnemy()
    {

        lowerBound = lowerBound + 2;
        upperBound = lowerBound + 3;

        totalEnemyCount = Random.Range(lowerBound, upperBound);


        int fasterMin = totalEnemyCount / 2;
        int fasterMax = (totalEnemyCount * 3) / 4;
        faster = Random.Range(fasterMin, fasterMax);

        totalEnemyCount -= faster;


        int rangerMin = totalEnemyCount / 2;
        int rangerMax = (totalEnemyCount * 3) / 4;
        ranger = Random.Range(rangerMin, rangerMax);

        totalEnemyCount -= ranger;

        int tankMin = totalEnemyCount / 2;
        int tankMax = (totalEnemyCount * 3) / 4;
        tank = Random.Range(tankMin, tankMax);

        enemyCount = faster + ranger + tank;

        int[] counts = { faster, ranger, tank };

        StartCoroutine(SpawnEnemy(counts));
    }
    IEnumerator SpawnEnemy(int[] counts)
    {
        int totalCounts = counts.Sum();

        while (totalCounts > 0)
        {
            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] > 0)
                {
                    GameObject Enemy = Instantiate(prefabs[i], RandomPos(), Quaternion.identity);
                    Enemy.transform.LookAt(player);
                    Enemy.transform.SetParent(enemies);
                    cloneEnemy = Enemy;
                    counts[i]--;
                    totalCounts--;

                    StartCoroutine(EnemyCreateAnimation(cloneEnemy));
                    yield return new WaitForSeconds(.25f);

                }
            }
        }
    }

    IEnumerator EnemyCreateAnimation(GameObject enemy)
    {
        while (enemy.transform.position.y <= 0)
        {
            enemy.transform.position += new Vector3(0, Time.deltaTime, 0);
            yield return null;
        }
    }
    private Vector3 RandomPos()
    {
        float spawnPosX = Random.Range(200, 330);
        float spawnPosZ = Random.Range(-60, 60);
        Vector3 randomPosition = new(spawnPosX, -2.5f, spawnPosZ);
        return randomPosition;
    }
}