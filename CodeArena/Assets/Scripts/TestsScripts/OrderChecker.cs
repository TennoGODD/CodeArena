using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OrderChecker : MonoBehaviour
{
    [SerializeField] private List<Holder> holders;
    [SerializeField] private Button nextButton;
    
    // Изменяем на поле с [SerializeField] для доступа в инспекторе
    [SerializeField] private bool isCorrectOrder;
    public bool IsCorrectOrder => isCorrectOrder; // Только чтение

    private void Start()
    {
        if (nextButton != null)
            nextButton.interactable = false;
    }

    public void CheckOrder()
    {
        bool allCorrect = true;
        ResetAllColors();
        
        foreach (var holder in holders)
        {
            if (holder == null) continue;
            
            if (holder.CheckCorrectItem())
            {
                HighlightHolder(holder, Color.green);
            }
            else
            {
                HighlightHolder(holder, Color.red);
                allCorrect = false;
            }
        }
        
        isCorrectOrder = allCorrect; // Изменяем приватное поле
        
        if (nextButton != null)
            nextButton.interactable = allCorrect;

        Debug.Log(allCorrect ? "Все верно! Кнопка разблокирована" : "Есть ошибки");
    }

    // Метод для сброса состояния (вызывается из MenuChanger)
    public void ResetOrderCheck()
    {
        isCorrectOrder = false;
        if (nextButton != null)
            nextButton.interactable = false;
        ResetAllColors();
    }

    private void HighlightHolder(Holder holder, Color color)
    {
        if (holder.transform.childCount > 0)
        {
            var image = holder.transform.GetChild(0).GetComponent<Image>();
            if (image != null) image.color = color;
        }
    }

    private void ResetAllColors()
    {
        foreach (var holder in holders)
        {
            if (holder != null && holder.transform.childCount > 0)
            {
                var image = holder.transform.GetChild(0).GetComponent<Image>();
                if (image != null) image.color = Color.white;
            }
        }
    }
}