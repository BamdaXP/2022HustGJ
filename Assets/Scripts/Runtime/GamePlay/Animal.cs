using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public Animaldata data;
    int j;
    // Start is called before the first frame update
    void Start()
    {
        j = data.change.Length;
        data.lev = new Animaldata.level[j];
        for (int i = 0; i < j; i++)
        {
            data.lev[i].changeDuration = data.change[i];
            data.lev[i].firMove = data.fir[i];
            data.lev[i].secMove = data.sec[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
