using UnityEngine;

public class GateOpener : MonoBehaviour
{
    public Transform leftDoor; 
    public Transform rightDoor;
    public Transform leftOpenPoint; 
    public Transform rightOpenPoint; 
    public float openSpeed = 2f;
    public float openDistance = 10f; 
    public Transform cameraTransform;

    private Vector3 leftClosedPosition;
    private Vector3 rightClosedPosition;

    private bool isOpening = false;

    void Start()
    {
        leftClosedPosition = leftDoor.position;
        rightClosedPosition = rightDoor.position;
    }

    void Update()
    {
        float distance = Vector3.Distance(cameraTransform.position, transform.position);

        if (distance < openDistance && !isOpening)
        {
            isOpening = true;
        }

        if (isOpening)
        {
            leftDoor.position = Vector3.Lerp(leftDoor.position, leftOpenPoint.position, openSpeed * Time.deltaTime);
            rightDoor.position = Vector3.Lerp(rightDoor.position, rightOpenPoint.position, openSpeed * Time.deltaTime);
        }
    }
}
