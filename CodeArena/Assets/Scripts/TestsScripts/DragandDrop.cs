using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragandDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Image image;

    // Свойства для доступа к parentToReturnTo и startPosition
    public Transform ParentToReturnTo { get; private set; }
    public Vector2 StartPosition { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.color = new Color(0f, 255f, 200f, 0.7f);
        image.raycastTarget = false; // Отключаем Raycast для TextBox
        ParentToReturnTo = transform.parent; // Сохраняем текущего родителя
        StartPosition = rectTransform.anchoredPosition; // Сохраняем начальную позицию
        transform.SetParent(transform.root); // Устанавливаем родителем корневой объект, чтобы объект был поверх всех других
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.color = new Color(255f, 255f, 255f, 1f);
        image.raycastTarget = true; // Включаем Raycast для TextBox

        // Проверяем, был ли блок отпущен над Holder'ом
        bool isOverHolder = false;
        foreach (var holder in FindObjectsOfType<Holder>())
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(holder.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera))
            {
                Debug.Log("Dropped over Holder: " + holder.gameObject.name);
                holder.AcceptBlock(this);
                isOverHolder = true;
                break;
            }
        }

        if (!isOverHolder)
        {
            Debug.Log("Dropped outside Holder");
            ReturnToStart();
        }
    }

    public void ReturnToStart()
    {
        transform.SetParent(ParentToReturnTo);
        rectTransform.anchoredPosition = StartPosition;
    }
}