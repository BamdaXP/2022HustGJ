using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Dialogue d1;
    public Dialogue d2;
    public DialoguePanel dialoguePanel;
    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel.AddDialogue(d1);
        dialoguePanel.AddDialogue(d2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
