using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class PawnSpeechBubble : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public void RunDialogue(string dialogue)
    {
        dialogueRunner.StartDialogue(dialogue);
    }
}
