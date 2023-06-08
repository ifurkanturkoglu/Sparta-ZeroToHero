using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAroundMap : MonoBehaviour
{
    [SerializeField] int radius;
    [SerializeField] int speed;
    [SerializeField] int rotSpeed;
    Vector3 origin;
    private void Start()
    {
        origin = transform.position + Quaternion.identity * Vector3.right * radius;
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward * rotSpeed);
        transform.RotateAround(origin, Vector3.up, speed * Time.deltaTime);
    }
}
