using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosChanger : MonoBehaviour
{
    public GameObject[] textBlocks; // Массив GameObject с текстовыми блоками
    private int selectedIndex = 0; // Индекс выбранного блока

    void Start()
    {
        // Выделяем первый блок при старте
        //HighlightSelectedBlock();
    }


    void Update()
    {

    }

    // Перемещение блока вверх
    public void MoveBlockUp()
    {
        if (selectedIndex > 0)
        {
            // Меняем местами позиции GameObject
            Vector3 tempPosition = textBlocks[selectedIndex].transform.position;
            textBlocks[selectedIndex].transform.position = textBlocks[selectedIndex - 1].transform.position;
            textBlocks[selectedIndex - 1].transform.position = tempPosition;

            // Обновляем выделение
            selectedIndex--;
            //HighlightSelectedBlock();
        }
    }

    // Перемещение блока вниз
    public void MoveBlockDown()
    {
        if (selectedIndex < textBlocks.Length - 1)
        {
            // Меняем местами позиции GameObject
            Vector3 tempPosition = textBlocks[selectedIndex].transform.position;
            textBlocks[selectedIndex].transform.position = textBlocks[selectedIndex + 1].transform.position;
            textBlocks[selectedIndex + 1].transform.position = tempPosition;

            // Обновляем выделение
            selectedIndex++;
            //HighlightSelectedBlock();
        }
    }

    // Выделение выбранного блока
    // void HighlightSelectedBlock()
    // {
    //     for (int i = 0; i < textBlocks.Length; i++)
    //     {
    //         TextMeshProUGUI textComponent = textBlocks[i].GetComponent<TextMeshProUGUI>();
    //         if (textComponent != null)
    //         {
    //             if (i == selectedIndex)
    //             {
    //                 textComponent.color = Color.red; // Выделяем красным
    //             }
    //             else
    //             {
    //                 textComponent.color = Color.black; // Остальные блоки черные
    //             }
    //         }
    //     }
    // }
}
