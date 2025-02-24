using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider attackCollider; // Ссылка на коллайдер атаки
    [SerializeField] private int attackDamage = 10; // Урон от атаки
    [SerializeField] private Vector3 attackColliderRightPosition; // Положение коллайдера, когда персонаж смотрит вправо
    [SerializeField] private Vector3 attackColliderLeftPosition;  // Положение коллайдера, когда персонаж смотрит влево

    private float speed = 2f;
    private float jumpForce = 5f;
    private bool isGrounded;
    private Rigidbody rb;
    private Animator animator;
    private PlayerInputActions playerInputActions;

    private bool allowRotation = true;
    private bool canMove = true;
    private bool isAttacking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        animator = GetComponentInChildren<Animator>();

        // Отключаем коллайдер атаки по умолчанию
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    private void OnEnable()
    {
        playerInputActions.Player.Jump.performed += _ => Jump();
        playerInputActions.Player.Attack.performed += _ => Attack(); // Вызов атаки при нажатии
    }

    private void OnDisable()
    {
        playerInputActions.Player.Jump.performed -= _ => Jump();
        playerInputActions.Player.Attack.performed -= _ => Attack();
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

        // Обновляем положение коллайдера атаки
        UpdateAttackColliderPosition();

        UpdateAnimation(inputVector);
    }

    private void UpdateAttackColliderPosition()
    {
        if (attackCollider != null)
        {
            // Выбираем положение коллайдера в зависимости от направления персонажа
            Vector3 targetPosition = spriteRenderer.flipX ? attackColliderLeftPosition : attackColliderRightPosition;

            // Устанавливаем новое положение коллайдера
            attackCollider.transform.localPosition = targetPosition;
        }
    }

    private void UpdateAnimation(Vector2 inputVector)
    {
        bool IsRunning = inputVector.magnitude > 0;
        if (canMove)
        {
            animator.SetBool("IsRunning", IsRunning);
        }
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsAttacking", isAttacking);
    }

    private void Jump()
    {
        if (isGrounded && canMove)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }
    }

    private void Attack()
    {
        if (canMove && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");

            // Включаем коллайдер атаки
            if (attackCollider != null)
            {
                attackCollider.enabled = true;
            }

            // Запускаем проверку на наличие врагов в зоне атаки
            StartCoroutine(CheckForEnemies());
        }
    }

    private IEnumerator CheckForEnemies()
    {
        // Ждем небольшой задержки перед проверкой
        yield return new WaitForSeconds(0.2f);

        // Наносим урон всем врагам в зоне атаки
        if (attackCollider != null)
        {
            Collider[] hitColliders = Physics.OverlapBox(attackCollider.bounds.center, attackCollider.bounds.extents, attackCollider.transform.rotation);

            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    if (collider.TryGetComponent(out Enemy enemyComponent))
                    {
                        enemyComponent.TakeDamage(attackDamage);
                        Debug.Log("Enemy took damage: " + attackDamage);
                    }
                }
            }
        }

        // Отключаем коллайдер атаки после завершения
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }

        // Сбрасываем флаг атаки после завершения
        yield return new WaitForSeconds(0.3f); // Время анимации атаки
        isAttacking = false;
        Debug.Log("Attack finished");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void SetAllowRotation(bool value)
    {
        allowRotation = value;
    }
}