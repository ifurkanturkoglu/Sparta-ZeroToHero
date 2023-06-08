using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSpike : MonoBehaviour
{
    // 200 330 - 240 290 x
    // z 60 -60 -30 30
    [SerializeField] GameObject spike;
    private void Update()
    {
        //SpawnSpike();
    }

    void SpawnSpike()
    {
        Instantiate(spike, RandomPoint(), transform.rotation);
    }

    Vector3 RandomPoint()
    {
        int x = Random.Range(200, 330);
        int z = Random.Range(-60, 60);

        Vector3 spawnPoint = new(x, 3, z);
        return spawnPoint;

    }
}
