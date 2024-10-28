using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;
    private float _speed = 0.05f;
 
    void Start()
    {
        
    }

    void Update()
    {
        
        for (int i = 0; i < _wayPoints.Length; i ++)
        {
            if(transform.position != _wayPoints[i].position)
            {
                var direction = (transform.position - _wayPoints[i].position).normalized;
                transform.Translate(direction * _speed);
                transform.LookAt(_wayPoints[i]);
            }
            
        }
    }
}
