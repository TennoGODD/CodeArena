using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float speed = 2f;
    private Rigidbody rb;
    private Animator animator;
    private PlayerInputActions playerInputActions;

    private bool allowRotation = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        animator = GetComponentInChildren<Animator>();
    }

    private Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = GetMovementVector();
        inputVector = inputVector.normalized;
        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y);
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        if (allowRotation && inputVector.x != 0)
        {
            spriteRenderer.flipX = inputVector.x > 0;
        }
        UpdateAnimation(inputVector);
    }

    private void UpdateAnimation(Vector2 inputVector)
    {
        bool IsRunning = inputVector.magnitude > 0; 
        animator.SetBool("IsRunning", IsRunning);
    }
    public void SetAllowRotation(bool value)
    {
        allowRotation = value;
    }
}