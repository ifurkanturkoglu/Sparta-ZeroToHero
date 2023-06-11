using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAroundMap : MonoBehaviour
{
    [SerializeField] int radius;
    [SerializeField] int speed;
    [SerializeField] int rotSpeed;
     int damage = 5;
    Vector3 origin;
    private void Start()
    {
        origin = transform.position + Quaternion.identity * Vector3.right * radius;
    }
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotSpeed);
        transform.RotateAround(origin, Vector3.up, speed * Time.deltaTime);
    }
     void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<Enemy>() !=null){
            other.gameObject.GetComponent<Enemy>().health -=damage;
        }
         if(other.gameObject.GetComponent<Player>() !=null){
            PlayerController.Instance.PlayerTakeDamage(null,damage);
        }
    }
}
