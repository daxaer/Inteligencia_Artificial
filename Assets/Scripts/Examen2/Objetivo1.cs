using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetivo1 : MonoBehaviour
{
    public bool touchingCube = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Agente"))
        touchingCube = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Agente"))
            touchingCube = false;
    }
}
