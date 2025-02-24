using UnityEngine;
using System.Collections;

public class SceneStartTeleport : MonoBehaviour
{
    [SerializeField] private Player player; // ������ �� ������
    [SerializeField] private GameObject teleportEffect; // ������ ������������
    [SerializeField] private float teleportEffectDuration = 4f; // ����� ������������ ������� ������������
    [SerializeField] private float playerAppearDelay = 2f; // �������� ����� ���������� ������

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
        // ������� ������ ������������ �� ������� ������
        GameObject effect = Instantiate(teleportEffect, player.transform.position, player.transform.rotation);

        // ����� ���������� ��������� (�� ����� � �� ����� ���������)
        player.gameObject.SetActive(false);
        player.SetCanMove(false);

        // ���� �������� ����� ���������� ������
        yield return new WaitForSeconds(playerAppearDelay);

        // ���������� ������ (����������)
        player.gameObject.SetActive(true);

        // ���� ���������� ����� ������� ������������
        yield return new WaitForSeconds(teleportEffectDuration - playerAppearDelay);

        // ���������� ������ ������������
        Destroy(effect);

        // ������ ����� ����� ���������
        player.SetCanMove(true);
    }
}