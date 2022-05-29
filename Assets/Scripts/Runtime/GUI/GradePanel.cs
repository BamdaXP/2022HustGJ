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
    public TMPro.TMP_Text text1;
    public TMPro.TMP_Text text2;
    public TMPro.TMP_Text text3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(2f);
        finish.transform.DOScale(1.25f, 2f).SetLoops(-1, LoopType.Yoyo);
        var words1 = "    今天共检测12位居民";
        var words2 = "  满意率100%";
        var words3 = "感谢您为居民健康做出的贡献！";
        var type = "";
        foreach (var c in words1)
        {
            type += c;
            text1.text = type;
            AudioManager.Instance.PlaySE("Type");
            yield return new WaitForSeconds(0.1f);
        }
        type = "";
        foreach (var c in words2)
        {
            type += c;
            text2.text = type;
            AudioManager.Instance.PlaySE("Type");
            yield return new WaitForSeconds(0.1f);
        }
        type = "";
        foreach (var c in words3)
        {
            type += c;
            text3.text = type;
            AudioManager.Instance.PlaySE("Type");
            yield return new WaitForSeconds(0.1f);
        }


        yield return new WaitForSeconds(1f);
        //finish.transform.DOLocalRotate(new Vector3(0f,0f,30f), Random.Range(1, 5)).SetLoops(-1,LoopType.Yoyo);
        var seq = DOTween.Sequence();
        seq.PrependInterval(1f);
        seq.Append(seal.DOScale(1f, 1f));
        seq.Join(DOTween.To(() => seal.GetComponent<Image>().color, c => seal.GetComponent<Image>().color = c, Color.white, 0.5f));
        seq.Append(certification.transform.DOScale(1.25f, 0.2f));
        seq.Append(certification.transform.DOScale(1f, 0.1f));
        seq.Append(certification.transform.DOScale(1.1f, 0.15f));
        seq.Append(nextButton.transform.DOLocalMoveY(-170, 1.5f));
        seq.Append((titleButton.transform.DOLocalMoveY(-340, 1.5f)));
        seq.Play();

        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlaySE("Seal");
    }
    public void ToTitle()
    {
        AudioManager.Instance.PlaySE("Button2");
        SceneLoader.Instance.LoadSceneAsync("TitleScene");
        SceneLoader.Instance.UnloadSceneAsync("GradeScene");
    }

    public void Next()
    {
        AudioManager.Instance.PlaySE("Button2");
        SceneLoader.Instance.LoadSceneAsync("GameScene");
        SceneLoader.Instance.UnloadSceneAsync("GradeScene");
    }
}
