using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverObjetivo : MonoBehaviour
{
    float random;
    private void Start()
    {
        Mover();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collicion pared");
        if (collision.collider.CompareTag("Pared"))
        {
            Mover();
        }
    }
    public void Mover()
    {
        random = Random.Range(-5, 5);
        transform.localPosition = new Vector3(random, 0.02f, random);
    }
}
