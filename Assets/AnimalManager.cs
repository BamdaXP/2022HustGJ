using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimalManager : Singleton<AnimalManager>
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
            endAnimal.transform.DOMove(endPoint.position, 3);
            endAnimal.transform.DOScaleY(1.05f, 0.3f).SetLoops(10, LoopType.Yoyo);
        }

        testAnimal = backupAnimal;
        if (testAnimal != null)
        {
            testAnimal.transform.DOMove(testPoint.position, 3);
            testAnimal.transform.DOScaleY(1.05f, 0.3f).SetLoops(10, LoopType.Yoyo);
            StartCoroutine(SetSpriteAfterTime(testAnimal, false, 3));
        }

        backupAnimal = initAnimal;
        if (backupAnimal != null)
        {
            backupAnimal.transform.DOMove(backupPoint.position, 3);
            backupAnimal.transform.DOScaleY(1.05f, 0.3f).SetLoops(10, LoopType.Yoyo);
        }

        if (datas.Count > 0)
        {
            initAnimal = Instantiate<GameObject>(animalPfb, transform).GetComponent<Animal>();
            initAnimal.transform.position = initPoint.position;
            initAnimal.sr.sortingOrder = datas.Count;
            datas.RemoveAt(0);
        }
        else
        {
            initAnimal = null;
        }
    }

    private IEnumerator SetSpriteAfterTime(Animal animal, bool mask, float time)
    {
        yield return new WaitForSeconds(time);
        animal.ChangeSprite(mask);
    }
}

