using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Emotion : MonoBehaviour
{
    public Sprite good;
    public Sprite bad;

    private SpriteRenderer sr;
    public enum EmotionType
    {
        Good,
        Bad
    }
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        //MakeEmotion(EmotionType.Good);
    }

    public void MakeEmotion(EmotionType type)
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(0.75f, 0.5f));
        seq.AppendInterval(1);
        seq.Append(transform.DOScale(0f, 0.5f));
        switch (type)
        {
            case EmotionType.Good:
                sr.sprite = good;
                break;
            case EmotionType.Bad:
                sr.sprite = bad;
                break;
            default:
                sr.sprite = null;
                break;
        }

        seq.Play();
    }
}
