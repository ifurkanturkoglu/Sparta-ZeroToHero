using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpike : MonoBehaviour
{
    [SerializeField] int speed;
    void Update()
    {
        transform.Rotate(Vector3.up * speed);
    }
}
