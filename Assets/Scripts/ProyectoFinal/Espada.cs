using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    public GameObject agente;
    
    void Start()
    {
        Physics.IgnoreCollision(agente.GetComponent<SphereCollider>(), GetComponent<CapsuleCollider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Agente"))
        {
            other.GetComponent<Dungeon>().restarPuntos();
            other.GetComponent<Dungeon>().MoverSpawn();
        }
    }
}
