using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public static Gold instance;
    public int gold;
    public int speed;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.UpdateGold(gold);
            Destroy(gameObject);
        }
    }
}

