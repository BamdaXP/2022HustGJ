using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "Animal")]
public class Animaldata : ScriptableObject
{
    //第一关
    public float height;
    public float heightRange;
    public float heightTime;
    //第二关
    //判定区间
    public float[] change;
    public float[] fir;
    public float[] sec;
    //控制器速度
    public float[] radTime;
    public float[] radSpeed;
    //场景速度
    public float[] sceTime;
    public float[] sceSpeed;
    public float keepTime;
    public float maxTime;
    public Vector3 slideStart_L;
    public Vector3 slideStart_R;
    public float hitDis;
    //wait状态的动画
}
