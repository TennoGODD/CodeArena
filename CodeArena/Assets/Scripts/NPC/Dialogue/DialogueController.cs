using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NPCNameText;
    [SerializeField] private TextMeshProUGUI NPCDialogueText;
    [SerializeField] private float typeSpeed = 10f;
    [SerializeField] private string nextSceneName;
    [SerializeField] private Player player;
    [SerializeField] private GameObject aiObject; // Ссылка на объект ИИ
    [SerializeField] private GameObject aiEffects;
    //[SerializeField] private Transform aiStartPoint; // Начальная позиция ИИ
    [SerializeField] private Transform aiEndPoint; // Конечная точка для движения ИИ
    [SerializeField] private float moveSpeed = 3f; // Скорость движения ИИ
    [SerializeField] private Camera mainCamera;  // Камера
    [SerializeField] private float shakeAmount = 0.1f;  // Сила тряски
    [SerializeField] private float shakeDuration = 0.5f;  // Длительность тряски
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject wallEffect;
    [SerializeField] private GameObject wallEffectPoint;
    private bool conversationEnded;
    private bool isTyping;
    private Coroutine typeDialogueCoroutine;
    private Queue<string> paragraphs = new Queue<string>();
    private const float MAX_TYPE_TIME = 0.1f;

    private bool aiHasAppeared = false; // Флаг, чтобы контролировать появление ИИ
    private bool isMovingAI = false; // Флаг для того, чтобы начать движение ИИ
    private int currentParagraphIndex = 0; // Индекс текущего параграфа

    private Vector3 originalCameraPosition;  // Начальная позиция камеры
    private bool isShaking = false;  // Флаг, чтобы не начать тряску снова, если она уже активирована

    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        if (paragraphs.Count == 0)
        {
            if (!conversationEnded)
            {
                StartConversation(dialogueText);
            }
            else if (conversationEnded && !isTyping)
            {
                EndConversation();
                return;
            }
        }

        if (!isTyping)
        {
            string p = paragraphs.Dequeue();
            currentParagraphIndex++; // Увеличиваем индекс текущего параграфа
            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(p));
        }

        if (currentParagraphIndex == 4 && !aiHasAppeared) // После четвертого параграфа
        {
            StartAIMovement(); // Начинаем движение ИИ
            StartCoroutine(ShakeScreen());  // Начинаем тряску экрана
            Instantiate(wallEffect, wallEffectPoint.transform.position, wallEffectPoint.transform.rotation);
            aiEffects.SetActive(true);
            Destroy(wall);
        }

        if (paragraphs.Count == 0)
        {
            conversationEnded = true;
        }
    }

    private void StartConversation(DialogueText dialogueText)
    {
        if (player != null)
        {
            player.SetCanMove(false); // Блокируем движение игрока
        }

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        NPCNameText.text = dialogueText.speakerName;
        for (int i = 0; i < dialogueText.paragraphs.Length; i++)
        {
            paragraphs.Enqueue(dialogueText.paragraphs[i]);
        }
    }

    private void StartAIMovement()
    {
        aiHasAppeared = true; // Отметим, что ИИ появился
        //aiObject.SetActive(true); // Активируем объект ИИ
        isMovingAI = true; // Начинаем двигать ИИ
    }

    private void Update()
    {
        if (isMovingAI)
        {
            MoveAI();
        }
    }

    private void MoveAI()
    {
        // Двигаем ИИ к конечной точке
        if (aiObject != null && aiEndPoint != null)
        {
            aiObject.transform.position = Vector3.MoveTowards(aiObject.transform.position, aiEndPoint.position, moveSpeed * Time.deltaTime);

            // Проверяем, если ИИ достиг конечной точки, останавливаем движение
            if (Vector3.Distance(aiObject.transform.position, aiEndPoint.position) < 0.1f)
            {
                isMovingAI = false; // Прекращаем движение
            }
        }
    }

    private void EndConversation()
    {
        if (player != null)
        {
            player.SetCanMove(true); // Разблокируем движение
        }

        paragraphs.Clear();
        conversationEnded = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        // Переход на следующую сцену
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private IEnumerator TypeDialogueText(string p)  // Убираем <string> из IEnumerator
    {
        isTyping = true;
        int maxVisibleChars = 0;

        NPCDialogueText.text = p;
        NPCDialogueText.maxVisibleCharacters = maxVisibleChars;

        foreach (char c in p.ToCharArray())
        {
            maxVisibleChars++;
            NPCDialogueText.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);  // Возвращаем WaitForSeconds, так как мы не возвращаем строки
        }

        isTyping = false;
    }

    private IEnumerator ShakeScreen()
    {
        if (isShaking) yield break;

        isShaking = true;
        originalCameraPosition = mainCamera.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float xOffset = Random.Range(-shakeAmount, shakeAmount);
            float yOffset = Random.Range(-shakeAmount, shakeAmount);
            mainCamera.transform.position = new Vector3(originalCameraPosition.x + xOffset, originalCameraPosition.y + yOffset, originalCameraPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.position = originalCameraPosition;  // Восстанавливаем исходную позицию камеры
        isShaking = false;
    }
}
