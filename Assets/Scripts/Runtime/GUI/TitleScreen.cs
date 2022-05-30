using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TitleScreen : MonoBehaviour
{
    public Transform animals;

    private void Start()
    {
        var seq = DOTween.Sequence();
        seq.Append(animals.transform.DOScaleY(1.05f,0.6f));
        seq.Append(animals.transform.DOScaleY(1f, 0.2f));
        seq.SetLoops(-1);
        seq.Play();
    }
    public void StartStroyGame()
    {
        print("load game");
        SceneLoader.Instance.LoadSceneAsync("GameScene");
        SceneLoader.Instance.UnloadSceneAsync("TitleScene");
        AudioManager.Instance.PlaySE("Button3");
    }

    public void StartInfinteGame()
    {
        SceneLoader.Instance.LoadSceneAsync("GradeScene");
        SceneLoader.Instance.UnloadSceneAsync("TitleScene");
        AudioManager.Instance.PlaySE("Button3");
    }
    public void QuitGame()
    {
        Application.Quit();
        AudioManager.Instance.PlaySE("Button");
    }
}
