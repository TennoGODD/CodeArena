using UnityEngine;

public class Holder : MonoBehaviour
{
    public void AcceptBlock(DragandDrop block)
    {
        Debug.Log("AcceptBlock called on " + gameObject.name);
        if (transform.childCount > 0)
        {
            Debug.Log("Swapping blocks");
            Transform existingBlock = transform.GetChild(0);
            existingBlock.SetParent(block.ParentToReturnTo);
            existingBlock.GetComponent<RectTransform>().anchoredPosition = block.StartPosition;
        }
        block.transform.SetParent(transform);
        block.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }
}