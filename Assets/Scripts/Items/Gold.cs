using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] AudioClip soundEffect;
    public static Gold instance;
    public int gold;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.UpdateGold(gold);
            AudioController.Instance.audioSource.PlayOneShot(soundEffect);
            Destroy(gameObject);
            
        }
    }
}

