using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetivo1 : MonoBehaviour
{
    public bool touchingAgent = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Agente"))
        touchingAgent = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Agente"))
            touchingAgent = false;
    }
}
