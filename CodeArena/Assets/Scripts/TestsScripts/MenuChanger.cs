using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuChanger : MonoBehaviour
{
    public GameObject[] menus;
    public OrderChecker orderChecker;
    public Button nextButton; // Кнопка "Вперед"
    public Button prevButton; // Кнопка "Назад"
    [SerializeField] private TextMeshProUGUI resultsText;
    
    private bool[] questionCompleted;
    private int currentIndex = 0;

    private void Start()
    {
        questionCompleted = new bool[menus.Length];
        SetActiveMenu(currentIndex);
        UpdateButtonsInteractable();
    }

    public void Next()
    {
        if (orderChecker != null && !orderChecker.IsCorrectOrder && currentIndex < menus.Length - 1)
        {
            Debug.Log("Сначала правильно расположите элементы!");
            return;
        }

        if (menus.Length == 0) return;

        if (currentIndex < menus.Length - 1)
        {
            menus[currentIndex].SetActive(false);
            currentIndex++;
            menus[currentIndex].SetActive(true);

            if (orderChecker != null)
                orderChecker.ResetOrderCheck();
        }

        UpdateButtonsInteractable();

        // Если перешли на окно результатов
        if (currentIndex == menus.Length - 1)
        {
            ShowResults();
        }
    }

    public void Previous()
    {
        if (menus.Length == 0) return;

        if (currentIndex > 0)
        {
            menus[currentIndex].SetActive(false);
            currentIndex--;
            menus[currentIndex].SetActive(true);
        }

        UpdateButtonsInteractable();
    }

    private void SetActiveMenu(int index)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(i == index);
        }
    }

    private void UpdateButtonsInteractable()
    {
        if (prevButton != null)
            prevButton.interactable = currentIndex > 0;

        if (nextButton != null)
        {
            // На окне результатов "вперед" можно оставить активным
            if (currentIndex == menus.Length - 1)
            {
                nextButton.interactable = true;
            }
            else
            {
                nextButton.interactable = questionCompleted != null 
                    && currentIndex >= 0 
                    && currentIndex < questionCompleted.Length 
                    && questionCompleted[currentIndex];
            }
        }
    }

    public void MarkQuestionAsCompleted(bool correctAnswer)
    {
        if (questionCompleted != null && currentIndex >= 0 && currentIndex < questionCompleted.Length)
        {
            questionCompleted[currentIndex] = correctAnswer;
        }
    }

    private void ShowResults()
    {
        if (resultsText == null) return;

        int correctCount = 0;
        int incorrectCount = 0;

        // Считаем только до последнего меню (результаты не считаем)
        for (int i = 0; i < menus.Length - 1; i++)
        {
            if (questionCompleted[i])
                correctCount++;
            else
                incorrectCount++;
        }

        resultsText.text = $"Результаты теста:\n\n" +
                           $"Правильных ответов: {correctCount}\n" +
                           $"Неправильных ответов: {incorrectCount}\n\n";

        for (int i = 0; i < menus.Length - 1; i++)
        {
            string status = questionCompleted[i] ? "✅ Верно" : "❌ Неверно";
            resultsText.text += $"Вопрос {i + 1}: {status}\n";
        }
    }
}
