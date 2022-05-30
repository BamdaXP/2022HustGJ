using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "Animal")]
public class AnimalData : ScriptableObject
{
    //��һ��
    public float height;
    public float heightRange;
    public float heightTime;
    //�ڶ���
    //�ж�����
    public float[] change;
    public float[] fir;
    public float[] sec;
    //�������ٶ�
    public float[] radTime;
    public float[] radSpeed;
    //�����ٶ�
    public float[] sceTime;
    public float[] sceSpeed;
    public float keepTime;
    public float maxTime;
    public Vector3 slideStart_L;
    public Vector3 slideStart_R;
    public float hitDis;
    //wait״̬�Ķ���

    public List<Dialogue> prelogs = new List<Dialogue>();
    public List<Dialogue> postlogs = new List<Dialogue>();

    public List<Sprite> cgs = new List<Sprite>();

    public Sprite maskSprite;
    public Sprite aSprite;


}
