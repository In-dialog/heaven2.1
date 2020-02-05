using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I have entered");
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("I am staying");
    //}

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("I have left");
    }

}
