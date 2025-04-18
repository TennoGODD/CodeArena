using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OrderChecker : MonoBehaviour
{
    [SerializeField] private List<Holder> holders; // Все холдеры на сцене
    


    public void CheckOrder()
    {
        bool allCorrect = true;
        
        // Сначала сбрасываем все цвета
        ResetAllColors();
        
        // Проверяем каждый холдер
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
        
        if (allCorrect)
        {
            Debug.Log("Все элементы на своих местах!");
        }
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