using System.Collections; // �������� ��� ������
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f; // �������� �����������
    [SerializeField] private float detectionRange = 5f; // �������� ����������� ������
    [SerializeField] private int collisionDamage = 10; // ���� ��� ������������
    [SerializeField] private int health = 50; // �������� ����������
    [SerializeField] private float knockbackForce = 5f; // ���� �������
    [SerializeField] private float damageCooldown = 0.5f; // ����� ���������� �������� ��� ��������� �����
    [SerializeField] private SpriteRenderer spriteRenderer; // ������ �� SpriteRenderer ��� ��������

    private Transform player; // ������ �� ������
    private Rigidbody rb; // ������ �� Rigidbody
    private Animator animator; // ������ �� Animator
    private bool isTakingDamage = false; // ���� ��������� �����

    private void Start()
    {
        // ������� ������ �� ����
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // �������� ��������� Rigidbody
        rb = GetComponent<Rigidbody>();

        // �������� ��������� Animator �� ��������� �������
        animator = GetComponentInChildren<Animator>();

        // ��������� �������� �� ���� X � Z, ����� ��������� �� ����� � �� ��������
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    private void Update()
    {
        if (player == null || isTakingDamage) return; // �� ���������, ���� �������� ����

        // ���������, ��������� �� ����� � ���� �����������
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // ���������� ������
            ChasePlayer();
            animator.SetBool("IsRunning", true); // �������� �������� Run
        }
        else
        {
            animator.SetBool("IsRunning", false); // �������� �������� Idle
        }
    }

    private void ChasePlayer()
    {
        // ����������� � ������
        Vector3 direction = (player.position - transform.position).normalized;

        // ��������� � ������
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // �������������� � ������ (���� �����)
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // ������� ������
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // ������� �����
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ����������� � �������, ������� ����
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out Player playerComponent))
            {
                playerComponent.TakeDamage(collisionDamage, transform.position); // �������� ������� ����������
                Debug.Log("Enemy dealt damage to player!");
            }
        }
    }

    public void TakeDamage(int damage, Vector3 playerPosition)
    {
        if (isTakingDamage) return; // ���������� ����, ���� ��� �������� ���

        health -= damage; // ��������� ��������
        Debug.Log("Enemy took damage: " + damage + ". Health: " + health);

        // ��������� �������� ��������� �����
        StartCoroutine(DamageEffect());

        // ������ ��� ��������� �����
        Knockback(playerPosition);

        // ���������, ���� �� ���������
        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageEffect()
    {
        isTakingDamage = true; // ��������� ��������

        // �������� �������
        float elapsedTime = 0f;
        while (elapsedTime < damageCooldown)
        {
            // �������� ������������ �������
            spriteRenderer.color = new Color(1, 1, 1, Mathf.PingPong(elapsedTime * 10, 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������� ���������� ���� �������
        spriteRenderer.color = Color.white;

        isTakingDamage = false; // ������������ ��������
    }

    private void Knockback(Vector3 playerPosition)
    {
        if (rb == null) return;

        // ����������� ������� (�� ������ � ����������)
        Vector3 knockbackDirection = (transform.position - playerPosition).normalized;

        // ������� ������� ����������
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // ���������� ����������
    }

    private void OnDrawGizmosSelected()
    {
        // ������������ ���� �����������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}