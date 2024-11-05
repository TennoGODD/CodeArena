using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NPCNameText;
    [SerializeField] private TextMeshProUGUI NPCDialogueText;
    [SerializeField] private float typeSpeed = 10f;
    private bool conversationEnded;
    private bool isTyping;
    private string p;
    private Coroutine typeDialogueCoroutine;
    private Queue<string> paragraphs = new Queue<string>();
    private const string HTML_ALPHA = "<color=#00000000>";
    private const float MAX_TYPE_TIME = 0.1f;

    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        if(paragraphs.Count == 0)
        {
            if(!conversationEnded)
            {
                StartConversation(dialogueText);
            }
            else if (conversationEnded && !isTyping)
            {
                EndConversation();
                return;
            }
        }
        if(!isTyping)
        {
            p = paragraphs.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(p));
        }
        else
        {
            //FinishParagraphEarly();
        }
        

        if(paragraphs.Count == 0)
        {
            conversationEnded = true;
        }
    }
    private void StartConversation(DialogueText dialogueText)
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        NPCNameText.text = dialogueText.speakerName;
        for (int i = 0; i < dialogueText.paragraphs.Length;i++)
        {
            paragraphs.Enqueue(dialogueText.paragraphs[i]);
        }
    }
    private void EndConversation()
    {
        paragraphs.Clear();

        conversationEnded = false;

        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        int maxVisibleChars = 0;

        NPCDialogueText.text = p;
        NPCDialogueText.maxVisibleCharacters = maxVisibleChars;        

        foreach (char c in p.ToCharArray())
        {

            maxVisibleChars++;
            NPCDialogueText.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }
    // private void FinishParagraphEarly()
    //{
    //        StopCoroutine(typeDialogueCoroutine);
    //    NPCDialogueText.text = p;
    //   isTyping = false;
    //}
}