using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTheLineController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("EndOfTheLine");
    }
}
