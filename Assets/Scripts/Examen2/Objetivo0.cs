using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetivo0 : MonoBehaviour
{
    public bool touchingAgent0 = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Agente"))
            touchingAgent0 = true;
    }
}
