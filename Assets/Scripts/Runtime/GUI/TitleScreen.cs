using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
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
