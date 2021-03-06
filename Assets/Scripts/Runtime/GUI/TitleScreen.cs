using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TitleScreen : MonoBehaviour
{
    public Transform animals;

    private void Start()
    {
        AudioManager.Instance.PlayBGM("BGM");

        var seq = DOTween.Sequence();
        seq.Append(animals.transform.DOScaleY(1.05f,0.6f));
        seq.Append(animals.transform.DOScaleY(1f, 0.2f));
        seq.SetLoops(-1);
        seq.Play();
    }
    public void StartStroyGame()
    {
        print("load game");
        GameManager.Instance.currentLevelName = "Level0";
        SceneLoader.Instance.LoadSceneAsync("Level0");
        SceneLoader.Instance.UnloadSceneAsync("TitleScene");
        AudioManager.Instance.PlaySE("Button");
        AudioManager.Instance.StopBGM("BGM");
    }

    public void StartInfinteGame()
    {
        GameManager.Instance.currentLevelName = "GameScene";
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
