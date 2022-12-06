using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverObjetivo : MonoBehaviour
{
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
        float random = Random.Range(-4, 4);
        float random2 = Random.Range(-4, 4);
        transform.localPosition = new Vector3(random, 0.02f, random2);
    }
}
