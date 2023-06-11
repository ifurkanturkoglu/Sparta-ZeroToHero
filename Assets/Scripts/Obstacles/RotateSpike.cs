using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpike : MonoBehaviour
{
    [SerializeField] int speed;
    int damage = 5;
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * speed);
    }

    void OnCollisionEnter(Collision other)
    {
        print(other.gameObject.name);
        if(other.gameObject.tag.Equals("Enemy")){
            print("girdidüşman");
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
         if(other.gameObject.tag.Equals("Player")){
            PlayerController.Instance.PlayerTakeDamage(null,damage);
        }
    }
}
