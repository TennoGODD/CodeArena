using UnityEngine;
using TMPro;
using System.Collections;

public class InstructorDialogue : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private float textDisplaySpeed = 0.05f;

    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;
    private int currentLineIndex = 0;
    private bool isTextDisplaying = false;
    private Coroutine displayTextCoroutine;

    public void PlayerEnteredTrigger()
    {
        isPlayerInRange = true;
        Debug.Log("Player is in range. Press E to talk.");
    }

    public void PlayerExitedTrigger()
    {
        isPlayerInRange = false;
        EndDialogue();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isDialogueActive)
        {
            StartDialogue();
        }

        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            ShowNextLine();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;
        dialogueText.text = "";
        if (displayTextCoroutine != null)
        {
            StopCoroutine(displayTextCoroutine);
        }
        displayTextCoroutine = StartCoroutine(DisplayText(dialogueLines[currentLineIndex]));
    }

    private IEnumerator DisplayText(string text)
    {
        isTextDisplaying = true;
        dialogueText.text = "";
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textDisplaySpeed);
        }
        isTextDisplaying = false;
    }

    private void ShowNextLine()
    {
        if (isTextDisplaying) return;

        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            dialogueText.text = "";
            if (displayTextCoroutine != null)
            {
                StopCoroutine(displayTextCoroutine);
            }
            displayTextCoroutine = StartCoroutine(DisplayText(dialogueLines[currentLineIndex]));
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        currentLineIndex = 0;
    }
}