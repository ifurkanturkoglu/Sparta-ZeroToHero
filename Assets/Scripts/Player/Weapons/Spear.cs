using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    int damage = 100;


    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Enemy>() != null){
            other.GetComponent<Enemy>().health -=damage;
        }
    }
}
