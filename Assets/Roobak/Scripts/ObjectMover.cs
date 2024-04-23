using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] Vector3 _moveDirection = new Vector3(0, 0, -5);
    [SerializeField] float _zDeadZone = 1;
    
    

    
    private void Update()
    {
        transform.Translate(_moveDirection * Time.deltaTime);
        if (transform.position.z < _zDeadZone)
        {

            transform.gameObject.SetActive(false);
        }
    }

    


}
