using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetivo1 : MonoBehaviour
{
    public bool touchingCube = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Cubo"))
        touchingCube = true;
    }
}
