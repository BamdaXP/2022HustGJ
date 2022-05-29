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
    public AnimalManager animalManager;
    public DialoguePanel dialoguePanel;

    public Doctor doctor;

    public PlayerState PlayerState;

    public GameObject firstStagePrefab;
    public GameObject secondStagePrefab;

    public GameObject firstStage;
    public GameObject secondStage;

    public Image CG;
    public List<Sprite> preCGs;
    public List<Sprite> postCGs;

    private void Start()
    {
        doctor.TurnAround(false);
        StartCoroutine(PreCG());
    }
    private bool end = false;
    private void Update()
    {
        if (animalManager.IsEmpty() && !end)
        {
            //������
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
            }
            CG.gameObject.GetComponent<Window>().Hide();
            yield return new WaitForSeconds(3f);
        }
        SceneLoader.Instance.UnloadSceneAsync("GameSceneTemp");
        SceneLoader.Instance.LoadSceneAsync("GradeScene");
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
                yield return null;
            }
        }
        
        

        yield return new WaitForSeconds(3f);
        
        
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
        SwitchGameState(PlayerState.Stage1);
        yield return null;
    }

    private IEnumerator DoStage1()
    {
        animalManager.testAnimal.ChangeSprite(false);
        /*
        // ������
        firstStage = Instantiate<GameObject>(firstStagePrefab, null, true);
        doctor.heightSource = firstStage.GetComponent<Stage1>();
        */
        var a = animalManager.testAnimal;
        if (a != null)
        {
            print("stage 1 for 3 s");
            yield return new WaitForSeconds(0.3f);
        }
        SwitchGameState(PlayerState.Stage2);
        yield return null;
        
    }

    private IEnumerator DoStage2()
    {
        doctor.heightSource = null;
        /*
        // ������
        Destroy(firstStage);
        secondStage = Instantiate<GameObject>(secondStagePrefab, null, true);
        doctor.depthSource = secondStage.GetComponent<Stage2>();
        //yield return new WaitForSeconds(2f);
        */
        var a = animalManager.testAnimal;
        if (a != null)
        {
            print("stage 2 for 3 s");
            yield return new WaitForSeconds(0.3f);
        }
        SwitchGameState(PlayerState.Dialog2);
        yield return null;
    }

    private IEnumerator DoDialog2()
    {
        animalManager.testAnimal.ChangeSprite(true);
        doctor.TurnAround(false);
        doctor.depthSource = null;
        //Destroy(secondStage);
        doctor.Init();
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
        SwitchGameState(PlayerState.Transition);
        yield return null;
    }

    private IEnumerator DoTransition()
    {
        // ������
        //yield return new WaitForSeconds(3f);
        animalManager.Proceed();
        SwitchGameState(PlayerState.Ready);
        yield return null;
    }
}
