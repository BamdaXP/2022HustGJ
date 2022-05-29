using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class GradePanel : MonoBehaviour
{
    public Transform certification;
    public Transform seal;
    public Transform finish;
    public Transform nextButton;
    public Transform titleButton;


    // Start is called before the first frame update
    void Start()
    {
        finish.transform.DOScale(1.2f, Random.Range(1, 5)).SetLoops(-1, LoopType.Yoyo);
        //finish.transform.DOLocalRotate(new Vector3(0f,0f,30f), Random.Range(1, 5)).SetLoops(-1,LoopType.Yoyo);
        var seq = DOTween.Sequence();
        seq.PrependInterval(1f);
        seq.Append(seal.DOScale(1f,1f));
        seq.Join(DOTween.To(() => seal.GetComponent<Image>().color, c => seal.GetComponent<Image>().color = c, Color.white, 0.5f));
        seq.Append(certification.transform.DOScale(1.1f, 0.2f));
        seq.Append(certification.transform.DOScale(0.9f, 0.1f));
        seq.Append(certification.transform.DOScale(1f, 0.15f));
        seq.Append(nextButton.transform.DOLocalMoveY(-170, 1.5f));
        seq.Append((titleButton.transform.DOLocalMoveY(-340, 1.5f)));
        seq.Play();
    }

    public void ToTitle()
    {
        SceneLoader.Instance.LoadSceneAsync("TitleScene");
        SceneLoader.Instance.UnloadSceneAsync("GradeScene");
    }

    public void Next()
    {
        SceneLoader.Instance.LoadSceneAsync("GameScene");
        SceneLoader.Instance.UnloadSceneAsync("GradeScene");
    }
}
