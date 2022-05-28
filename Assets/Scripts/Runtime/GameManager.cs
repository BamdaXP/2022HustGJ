using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        Title,
        First
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.LoadSceneAsync("TitleScene");
        AudioManager.Instance.PlayBGM("BGM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
