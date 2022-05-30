using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimalManager : MonoBehaviour
{
    public List<AnimalData> datas;
    //public List<Animal> animals;
    public GameObject animalPfb;

    public Transform initPoint;
    public Transform backupPoint;
    public Transform testPoint;
    public Transform endPoint;

    [HideInInspector]
    public Animal initAnimal;
    [HideInInspector]
    public Animal backupAnimal;
    [HideInInspector]
    public Animal testAnimal;
    [HideInInspector]
    public Animal endAnimal;

    public bool testing = false;
    public void Proceed()
    {
        if (endAnimal != null)
            Destroy(endAnimal.gameObject);
        endAnimal = testAnimal;
        if (endAnimal != null)
        {
            endAnimal.ChangeSprite(true);
            endAnimal.GetComponent<SpriteRenderer>().sortingLayerName = "CheckedAnimal";
            //endAnimal.transform.DOMove(endPoint.position, 3);
            endAnimal.transform.DOBlendableMoveBy(endPoint.position-endAnimal.transform.position, 3f);
            endAnimal.transform.DOBlendableLocalMoveBy(new Vector3(0, -0.15f, 0), 0.3f).SetLoops(10, LoopType.Yoyo);
        }

        testAnimal = backupAnimal;
        if (testAnimal != null)
        {
            testAnimal.transform.DOBlendableMoveBy(testPoint.position-testAnimal.transform.position, 3f);
            testAnimal.transform.DOBlendableLocalMoveBy(new Vector3(0, -0.15f, 0), 0.3f).SetLoops(10, LoopType.Yoyo);
        }

        backupAnimal = initAnimal;
        if (backupAnimal != null)
        {
            backupAnimal.transform.DOBlendableMoveBy(backupPoint.position-backupAnimal.transform.position, 3f);
            backupAnimal.transform.DOBlendableLocalMoveBy(new Vector3(0, -0.15f, 0), 0.3f).SetLoops(10, LoopType.Yoyo);
        }

        if (datas.Count > 0)
        {
            initAnimal = Instantiate<GameObject>(animalPfb, transform).GetComponent<Animal>();
            initAnimal.data = datas[0];
            initAnimal.transform.position = initPoint.position;
            //initAnimal.sr.sortingOrder = datas.Count;
            datas.RemoveAt(0);
        }
        else
        {
            initAnimal = null;
        }
    }

    public bool IsEmpty()
    {
        return datas.Count == 0 && initAnimal == null && backupAnimal == null && testAnimal == null;
    }
}

