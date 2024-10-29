using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private float _speed = 0.05f;
    [SerializeField] private float _rotationSpeed = 0.5f;
     private int _currentPoint = 0;

    void Update()
    {
        if (transform.position == _wayPoints[_currentPoint].position)
        {
            _currentPoint = (_currentPoint + 1) % _wayPoints.Length;
        }
        
        transform.position = Vector3.MoveTowards(transform.position,_wayPoints[_currentPoint].position,_speed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_wayPoints[_currentPoint].position), _rotationSpeed*Time.deltaTime);
    }
}
