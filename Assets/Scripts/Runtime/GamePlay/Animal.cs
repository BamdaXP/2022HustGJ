using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Animal : MonoBehaviour
{
    public AnimalData data;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = data.maskSprite;
    }

    public void ChangeSprite(bool mask)
    {
        if (mask) sr.sprite = data.maskSprite;
        else sr.sprite = data.aSprite;
    }
}
