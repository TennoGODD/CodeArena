using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public InstructorDialogue instructorDialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instructorDialogue.PlayerEnteredTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instructorDialogue.PlayerExitedTrigger();
        }
    }
}