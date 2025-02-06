using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void Update() 
    {
        Vector3 newPosition = transform.position;
        newPosition.x = playerTransform.position.x;
        transform.position = newPosition;
    }
}