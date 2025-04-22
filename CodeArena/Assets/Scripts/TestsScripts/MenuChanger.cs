using UnityEngine;
using UnityEngine.UI;

public class MenuChanger : MonoBehaviour
{
    public GameObject[] menus;
    public OrderChecker orderChecker;
    public Button nextButton; // Кнопка "Вперед"
    public Button prevButton; // Кнопка "Назад"
    
    private int currentIndex = 0;

    private void Start()
    {
        SetActiveMenu(currentIndex);
        UpdateButtonsInteractable();
    }

    public void Next()
    {
        if (orderChecker != null && !orderChecker.IsCorrectOrder && currentIndex != menus.Length - 1)
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
        // Блокируем "Назад" на первом элементе
        if (prevButton != null)
            prevButton.interactable = currentIndex > 0;

        // Блокируем "Вперед" на последнем элементе
        if (nextButton != null)
            nextButton.interactable = currentIndex < menus.Length - 1;
    }
}