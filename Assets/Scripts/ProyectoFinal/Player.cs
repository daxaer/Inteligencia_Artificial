using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float velocidadRotacion = 0.1f;
    public float random;

    private void Start()
    {
        MoverSpawn();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pared"))
        {
            {
                MoverSpawn();
            }
        }
    }
    public void MoverSpawn()
    {
        random = Random.Range(-5, 5);
        transform.localPosition = new Vector3(random, 0.2f, random);
    }
    public void FixedUpdate()
    {
        this.gameObject.transform.Rotate(Vector3.up * velocidadRotacion);
    }
}
