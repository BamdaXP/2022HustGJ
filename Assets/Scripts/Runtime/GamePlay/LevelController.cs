using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Ready,
    Dialog1,
    Stage1,
    Stage2,
    Dialog2,
    Transition,
}
public class LevelController : Singleton<LevelController>
{
    public string BGM;
    public AnimalManager animalManager;
    public DialoguePanel dialoguePanel;

    public Doctor doctor;

    public PlayerState PlayerState;

    //public GameObject firstStagePrefab;
    public GameObject secondStagePrefab;

    public Window firstStage;
    public GameObject secondStage;
    public Stage1 stage1;
    public Stage2 stage2;

    public Image CG;
    public List<Sprite> preCGs;
    public List<Sprite> postCGs;

    public Emotion emotion;

    private int currentScore;

    private void Start()
    {

        GameManager.Instance.LevelInit();
        AudioManager.Instance.PlayBGM(BGM);

        doctor.TurnAround(false);
        doctor.SetSweat(false);
        StartCoroutine(PreCG());
    }

    private bool end = false;
    private void Update()
    {
        if (animalManager.IsEmpty() && !end)
        {
            //测完了
            StopAllCoroutines();
            StartCoroutine(PostCG());
            end = true;
        }
    }
    private IEnumerator PreCG()
    {
        if (preCGs.Count > 0)
        {
            CG.gameObject.GetComponent<Window>().Show();
            foreach (var cg in preCGs)
            {
                CG.sprite = cg;
                yield return new WaitForSeconds(3f);
                while (!Input.GetMouseButton(0))
                {
                    yield return null;
                }
                AudioManager.Instance.PlaySE("Page");
            }
            CG.gameObject.GetComponent<Window>().Hide();
            yield return new WaitForSeconds(3f);

        }

        SwitchGameState(PlayerState.Ready);
    }

    private IEnumerator PostCG()
    {
        if (postCGs.Count > 0)
        {
            CG.gameObject.GetComponent<Window>().Show();
            foreach (var cg in postCGs)
            {
                CG.sprite = cg;
                yield return new WaitForSeconds(3f);
                while (!Input.GetMouseButton(0))
                {
                    yield return null;
                }
                AudioManager.Instance.PlaySE("Page");
            }
            CG.gameObject.GetComponent<Window>().Hide();
            yield return new WaitForSeconds(3f);
        }
        SceneLoader.Instance.UnloadSceneAsync(GameManager.Instance.currentLevelName);
        SceneLoader.Instance.LoadSceneAsync("GradeScene");
        AudioManager.Instance.StopBGM(BGM);
    }
    public void SwitchGameState(PlayerState state)
    {
        PlayerState = state;
        switch (state)
        {
            case PlayerState.Ready:
                StartCoroutine(GetReady());
                break;
            case PlayerState.Dialog1:
                StartCoroutine(DoDialog1());
                break;
            case PlayerState.Stage1:
                StartCoroutine(DoStage1());
                break;
            case PlayerState.Stage2:
                StartCoroutine(DoStage2());
                break;
            case PlayerState.Dialog2:
                StartCoroutine(DoDialog2());
                break;
            case PlayerState.Transition:
                StartCoroutine(DoTransition());
                break;
        }
    }

    private IEnumerator GetReady()
    {
        if (animalManager.datas.Count != 0)
        {
            while (animalManager.testAnimal == null)
            {
                animalManager.Proceed();
                yield return new WaitForSeconds(3.0f);
            }
        }
        
        
        SwitchGameState(PlayerState.Dialog1);
    }

    private IEnumerator DoDialog1()
    {
        doctor.TurnAround(true);
        var a = animalManager.testAnimal;
        if (a != null)
        {
            foreach (var pd in a.data.prelogs)
            {
                dialoguePanel.AddDialogue(pd);
                yield return null;
            }
        }
        while (dialoguePanel.IsDialoging)
        {
            yield return null;
        }
        //yield return new WaitForSeconds(2f);
        SwitchGameState(PlayerState.Stage1);
        yield return null;
    }

    private IEnumerator DoStage1()
    {
        if (GameManager.Instance.TestCount >= 3)
        {
            doctor.SetSweat(true);
        }
        currentScore = 0;
        animalManager.testAnimal.ChangeSprite(false);

        // 待更改
        //firstStage = Instantiate<GameObject>(firstStagePrefab, null, true);
        firstStage.Show();
        stage1.Init();
        doctor.heightSource = stage1;

        //var a = animalManager.testAnimal;
        yield return new WaitForSeconds(1f);
        //if (a != null)
        while (!stage1.Finished)
        {
            
            //print("stage 1 for 3 s");
            yield return new WaitForSeconds(0.3f);
        }
        currentScore += stage1.Score;
        firstStage.Hide();
        doctor.heightSource = null;
        SwitchGameState(PlayerState.Stage2);
        yield return null;
        
    }

    private IEnumerator DoStage2()
    {

        // 待更改
        //Destroy(firstStage);
        secondStage = Instantiate<GameObject>(secondStagePrefab, null, true);
        doctor.depthSource = secondStage.GetComponent<Stage2>();
        //yield return new WaitForSeconds(2f);

        //var a = animalManager.testAnimal;
        //if (a != null)
        //{
        //    print("stage 2 for 3 s");
        //    yield return new WaitForSeconds(0.3f);
        //}
        //secondStage.Show();
        //yield return new WaitForSeconds(2f);
        //secondStage.Hide();
        while (!doctor.depthSource.isFinishSecStep)
        {
            yield return null;
        }
        Destroy(secondStage);
        doctor.depthSource = null;
        SwitchGameState(PlayerState.Dialog2);
        yield return null;
    }

    private IEnumerator DoDialog2()
    {
        animalManager.testAnimal.ChangeSprite(true);
        doctor.TurnAround(false);
        //Destroy(secondStage);
        //doctor.Init();
        var a = animalManager.testAnimal;
        if (a != null)
        {
            foreach (var pd in a.data.postlogs)
            {
                dialoguePanel.AddDialogue(pd);
                yield return null;
            }
        }
        while (dialoguePanel.IsDialoging)
        {
            yield return null;
        }

        if (a != null)
        {
            if (a.data.cgs.Count > 0)
            {
                CG.gameObject.GetComponent<Window>().Show();
                foreach (var cg in a.data.cgs)
                {
                    CG.sprite = cg;
                    yield return new WaitForSeconds(3f);
                    while (!Input.GetMouseButton(0))
                    {
                        yield return null;
                    }
                    AudioManager.Instance.PlaySE("Page");
                }
                CG.gameObject.GetComponent<Window>().Hide();
                yield return new WaitForSeconds(3f);
            }
        }

        //yield return new WaitForSeconds(2f);
        SwitchGameState(PlayerState.Transition);
        yield return null;
    }

    private IEnumerator DoTransition()
    {
        // 待更改
        AudioManager.Instance.PlaySE("Button3");
        emotion.MakeEmotion(Emotion.EmotionType.Good);

        GameManager.Instance.TestCount++;
        GameManager.Instance.Score += currentScore;

        yield return new WaitForSeconds(2f);
        animalManager.Proceed();
        SwitchGameState(PlayerState.Ready);
        yield return null;
    }
}
