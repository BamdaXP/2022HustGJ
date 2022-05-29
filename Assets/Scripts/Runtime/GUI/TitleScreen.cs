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
        seq.Append(animals.transform.DOScaleY(1.05f,0.3f));
        seq.Append(animals.transform.DOScaleY(1f, 0.1f));
        seq.SetLoops(-1);
    }
    public void StartGame()
    {
        SceneLoader.Instance.LoadSceneAsync("GameScene");
        SceneLoader.Instance.UnloadSceneAsync("TitleScene");
        AudioManager.Instance.PlaySE("Button");
    }

    public void QuitGame()
    {
        Application.Quit();
        AudioManager.Instance.PlaySE("Button");
    }
}
