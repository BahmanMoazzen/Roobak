using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSelector : MonoBehaviour
{
    [SerializeField] GameObject[] _faces;
    int _currentFace = 0;

    private void OnEnable()
    {
        _currentFace = Random.Range(0, _faces.Length);
        _faces[_currentFace].SetActive(true);
    }

    private void OnDisable()
    {
        _faces[_currentFace].SetActive(false);
    }

    
}
