using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetivo2 : MonoBehaviour
{
    public bool touchingAgent = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Agente"))
            touchingAgent = true;
    }
}
