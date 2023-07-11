using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed;
    private void OnEnable()
    {
        StartCoroutine(DestroyArrow());
    }
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
    private IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(.8f);
        gameObject.SetActive(false);
    }
}
