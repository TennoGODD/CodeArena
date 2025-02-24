using UnityEngine;
using System.Collections;

public class SceneStartTeleport : MonoBehaviour
{
    [SerializeField] private Player player; // Ссылка на игрока
    [SerializeField] private GameObject teleportEffect; // Эффект телепортации
    [SerializeField] private float teleportEffectDuration = 4f; // Общая длительность эффекта телепортации
    [SerializeField] private float playerAppearDelay = 2f; // Задержка перед появлением игрока

    private void Start()
    {
        if (player != null && teleportEffect != null)
        {
            StartCoroutine(PlayTeleportEffectAtStart());
        }
        else
        {
            Debug.LogError("Player or Teleport Effect is not assigned!");
        }
    }

    private IEnumerator PlayTeleportEffectAtStart()
    {
        // Создаем эффект телепортации на позиции игрока
        GameObject effect = Instantiate(teleportEffect, player.transform.position, player.transform.rotation);

        // Игрок изначально неактивен (не виден и не может двигаться)
        player.gameObject.SetActive(false);
        player.SetCanMove(false);

        // Ждем задержку перед появлением игрока
        yield return new WaitForSeconds(playerAppearDelay);

        // Активируем игрока (появляется)
        player.gameObject.SetActive(true);

        // Ждем оставшееся время эффекта телепортации
        yield return new WaitForSeconds(teleportEffectDuration - playerAppearDelay);

        // Уничтожаем эффект телепортации
        Destroy(effect);

        // Теперь игрок может двигаться
        player.SetCanMove(true);
    }
}