using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float speed = 2f;
    private Rigidbody rb;
    private Animator animator;
    private PlayerInputActions playerInputActions;

    private bool allowRotation = true;
    private bool canMove = true;

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
        if (canMove)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }

        if (canMove && allowRotation && inputVector.x != 0)
        {
            spriteRenderer.flipX = inputVector.x > 0;
        }
        UpdateAnimation(inputVector);
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    private void UpdateAnimation(Vector2 inputVector)
    {
        bool IsRunning = inputVector.magnitude > 0;
        if (canMove)
        {
            animator.SetBool("IsRunning", IsRunning);
        }
    }
    public void SetAllowRotation(bool value)
    {
        allowRotation = value;
    }
}