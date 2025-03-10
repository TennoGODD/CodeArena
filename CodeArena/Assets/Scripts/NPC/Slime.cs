using System.Collections; // Добавьте эту строку
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f; // Скорость перемещения
    [SerializeField] private float detectionRange = 5f; // Диапазон обнаружения игрока
    [SerializeField] private int collisionDamage = 10; // Урон при столкновении
    [SerializeField] private int health = 50; // Здоровье противника
    [SerializeField] private float knockbackForce = 5f; // Сила отскока
    [SerializeField] private float damageCooldown = 0.5f; // Время блокировки движения при получении урона
    [SerializeField] private SpriteRenderer spriteRenderer; // Ссылка на SpriteRenderer для мерцания

    private Transform player; // Ссылка на игрока
    private Rigidbody rb; // Ссылка на Rigidbody
    private Animator animator; // Ссылка на Animator
    private bool isTakingDamage = false; // Флаг получения урона

    private void Start()
    {
        // Находим игрока по тегу
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Получаем компонент Rigidbody
        rb = GetComponent<Rigidbody>();

        // Получаем компонент Animator из дочернего объекта
        animator = GetComponentInChildren<Animator>();

        // Фиксируем вращение по осям X и Z, чтобы противник не падал и не крутился
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    private void Update()
    {
        if (player == null || isTakingDamage) return; // Не двигаемся, если получаем урон

        // Проверяем, находится ли игрок в зоне обнаружения
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Преследуем игрока
            ChasePlayer();
            animator.SetBool("IsRunning", true); // Включаем анимацию Run
        }
        else
        {
            animator.SetBool("IsRunning", false); // Включаем анимацию Idle
        }
    }

    private void ChasePlayer()
    {
        // Направление к игроку
        Vector3 direction = (player.position - transform.position).normalized;

        // Двигаемся к игроку
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Поворачиваемся к игроку (если нужно)
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Смотрим вправо
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Смотрим влево
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Если столкнулись с игроком, наносим урон
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out Player playerComponent))
            {
                playerComponent.TakeDamage(collisionDamage, transform.position); // Передаем позицию противника
                Debug.Log("Enemy dealt damage to player!");
            }
        }
    }

    public void TakeDamage(int damage, Vector3 playerPosition)
    {
        if (isTakingDamage) return; // Игнорируем урон, если уже получаем его

        health -= damage; // Уменьшаем здоровье
        Debug.Log("Enemy took damage: " + damage + ". Health: " + health);

        // Запускаем анимацию получения урона
        StartCoroutine(DamageEffect());

        // Отскок при получении урона
        Knockback(playerPosition);

        // Проверяем, умер ли противник
        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageEffect()
    {
        isTakingDamage = true; // Блокируем движение

        // Мерцание спрайта
        float elapsedTime = 0f;
        while (elapsedTime < damageCooldown)
        {
            // Изменяем прозрачность спрайта
            spriteRenderer.color = new Color(1, 1, 1, Mathf.PingPong(elapsedTime * 10, 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Возвращаем нормальный цвет спрайта
        spriteRenderer.color = Color.white;

        isTakingDamage = false; // Разблокируем движение
    }

    private void Knockback(Vector3 playerPosition)
    {
        if (rb == null) return;

        // Направление отскока (от игрока к противнику)
        Vector3 knockbackDirection = (transform.position - playerPosition).normalized;

        // Придаем импульс противнику
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // Уничтожаем противника
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация зоны обнаружения
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}