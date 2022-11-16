using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverObjetivo : MonoBehaviour
{
    private void Start()
    {
        Mover();
    }
    float random;
    private void OnTriggerEnter(Collider other)
    {
        random = Random.Range(-5,5);
        if(other.CompareTag("Pared"))
        {
            Mover();
        }
    }
    public void Mover()
    {
        transform.localPosition = new Vector3(random, 0.02f, random);
    }
}
