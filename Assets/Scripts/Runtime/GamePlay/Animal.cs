using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Animal : MonoBehaviour
{
    public AnimalData data;
    public SpriteRenderer sr;
    int j;
    // Start is called before the first frame update
    void Start()
    {
        sr.sprite = data.maskSprite;
    }

    public void StartTest()
    {
        sr.sprite = data.aSprite;
    }
    public void EndTest()
    {
        sr.sprite = data.maskSprite;
    }
}
