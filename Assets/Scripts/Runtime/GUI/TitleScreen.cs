using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.Instance.LoadSceneAsync("GameScene");
        AudioManager.Instance.PlaySE("Button");
    }

    public void QuitGame()
    {
        Application.Quit();
        AudioManager.Instance.PlaySE("Button");
    }
}
