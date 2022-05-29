using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public AnimalManager manager;

    public DialoguePanel p;
    public Dialogue d1;
    public Dialogue d2;
    // Start is called before the first frame update
    void Start()
    {
        p.AddDialogue(d1);
        p.AddDialogue(d2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            manager.Proceed();
        }
    }
}
