using UnityEngine;
using System.Collections.Generic;

public class OrderChecker : MonoBehaviour
{
    public List<Holder> holders; // Список всех Holders (задаём в инспекторе)
    public List<string> correctOrder; // Список правильного порядка (имена блоков)

    public void CheckOrder()
    {
        List<string> currentOrder = new List<string>();

        // Собираем текущий порядок блоков
        foreach (var holder in holders)
        {
            if (holder.transform.childCount > 0)
            {
                currentOrder.Add(holder.transform.GetChild(0).name);
            }
            else
            {
                currentOrder.Add("Empty"); // Если холдер пустой
            }
        }

        // Сравниваем с правильным порядком
        bool isCorrect = true;
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (currentOrder[i] != correctOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        // Вывод результата
        if (isCorrect)
        {
            Debug.Log("Порядок верный!");
        }
        else if (!isCorrect)
        {
            for (int i = 0; i < holders.Count; i++)
            {
                if (currentOrder[i] != correctOrder[i] && holders[i].transform.childCount > 0)
                {
                    holders[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.red;
                }
            }
        }
    }
}
