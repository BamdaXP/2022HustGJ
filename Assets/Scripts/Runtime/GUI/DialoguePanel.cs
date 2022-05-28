using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialoguePanel : MonoBehaviour
{
    public TMP_Text text;
    public Image panelImage;


    public bool IsDialoging { get; private set; }
    public bool WaitForNext { get; private set; }
    public Dialogue DialogingDialogue{ get; private set; }

public List<Dialogue> dialogueQueue;

    private Window _window;
    private void Awake()
    {
        _window = GetComponent<Window>();
        dialogueQueue = new List<Dialogue>();
    }
    public void AddDialogue(Dialogue dialogue)
    {
        dialogueQueue.Add(dialogue);

    }
    //Dont change alpha
    public void SetColor(Color c)
    {
        panelImage.color = new Color(c.r, c.g, c.b, panelImage.color.a);
    }
    private void Update()
    {
        if (_window.IsAnimating)
            return;

        if (dialogueQueue.Count > 0 && !IsDialoging)
        {
            IsDialoging = true;
            text.text = "";
            SetColor(Color.white);
            _window.Show();
        }
        else if (dialogueQueue.Count == 0 && IsDialoging)
        {
            IsDialoging = false;
            _window.Hide();
        }

        if (WaitForNext)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dialogueQueue.RemoveAt(0);
                DialogingDialogue = null;
                WaitForNext = false;
            }
        }
        
        if (DialogingDialogue == null && dialogueQueue.Count > 0)
        {
            DialogingDialogue = dialogueQueue[0];
            StartCoroutine(Print(DialogingDialogue));
        }
    }
    IEnumerator Print(Dialogue dialogue)
    {
        string showingText = "";
        SetColor(dialogue.dialogueColor);
        foreach (var c in dialogue.text)
        {
            showingText += c;
            text.text = showingText;
            yield return new WaitForSeconds(1f / (10 * dialogue.textSpeed));
        }
        WaitForNext = true;
    }


}
