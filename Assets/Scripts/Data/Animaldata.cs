using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "Animal")]
public class AnimalData : ScriptableObject
{
    //第一关
    public float height;
    public float heightRange;
    public float heightTime;
    //第二关
    public float[] change;
    public float[] fir;
    public float[] sec;

    public struct level
    {
        public float changeDuration;
        public float firMove;
        public float secMove;
    }
    public level[] lev;

    

    public List<Dialogue> prelogs = new List<Dialogue>();
    public List<Dialogue> dialogues = new List<Dialogue>();

    public Sprite maskSprite;
    public Sprite aSprite;
}
