using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    //public enum GameState
    //{
    //    Title,
    //    InGame,
    //    GameOver,
    //}

    //public GameState gameState = GameState.Title;
    // Start is called before the first frame update

    public int TestCount;
    public int Score;

    public void LevelInit()
    {
        TestCount = 0;
        Score = 0;
    }

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
