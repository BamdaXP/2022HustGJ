using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Ready,
    Dialog,
    Stage1,
    Stage2,
    Transition,
}
public class LevelController : Singleton<LevelController>
{
    public AnimalManager manager;

    public DoctorArmMove doctor;

    public PlayerState PlayerState;

    public GameObject firstStagePrefab;
    public GameObject secondStagePrefab;

    public GameObject firstStage;
    public GameObject secondStage;

    private void Start()
    {
        manager.Proceed();
        manager.Proceed();
        SwitchGameState(PlayerState.Ready);
    }

    public void SwitchGameState(PlayerState state)
    {
        PlayerState = state;
        switch (state)
        {
            case PlayerState.Ready:
                StartCoroutine(GetReady());
                break;
            case PlayerState.Dialog:
                //待更改
                SwitchGameState(PlayerState.Stage1);
                break;
            case PlayerState.Stage1:
                StartCoroutine(DoStage1());
                break;
            case PlayerState.Stage2:
                StartCoroutine(DoStage2());
                break;
            case PlayerState.Transition:
                StartCoroutine(DoTransition());
                break;
        }
    }

    private IEnumerator GetReady()
    {
        manager.Proceed();
        yield return new WaitForSeconds(3.1f);
        SwitchGameState(PlayerState.Dialog);
    }

    private IEnumerator DoStage1()
    {
        // 待更改
        firstStage = Instantiate<GameObject>(firstStagePrefab);
        doctor.heightSource = firstStage.GetComponent<VerticalCheck>();
        yield return null;
    }

    private IEnumerator DoStage2()
    {
        // 待更改
        doctor.heightSource = null;
        Destroy(firstStage);
        secondStage = Instantiate<GameObject>(secondStagePrefab);
        doctor.depthSource = secondStage.GetComponent<L_or_R>();
        yield return new WaitForSeconds(2f);
        SwitchGameState(PlayerState.Transition);
    }

    private IEnumerator DoTransition()
    {
        // 待更改
        doctor.depthSource = null;
        Destroy(secondStage);
        SwitchGameState(PlayerState.Ready);
        yield return null;
    }
}
