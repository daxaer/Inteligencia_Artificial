using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetivo3 : MonoBehaviour
{
    public bool touchingAgent2 = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Agente"))
            touchingAgent2 = true;
    }
}
