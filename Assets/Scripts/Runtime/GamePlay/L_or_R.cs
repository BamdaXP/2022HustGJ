using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_or_R : MonoBehaviour
{
    public GameObject stripe;//������ĳ���
    public GameObject slide;//��������ı߽�(�����)
    public GameObject slide1, slide2;
    public GameObject pos;//ָ��

    public bool isFinishFirStep;//��һ���Ƿ����(�߶ȿ��Ʋ���)
    public bool isTired;//�Ƿ���ƣ��״̬
    public bool isFinishSecStep;
    bool isEnter;//�Ƿ�����������(�ı������ʽ)
    bool isBulid;//����UI��ʾ
    bool isMoving;//�����ж����ƶ�
    bool isOver;//�Ƿ��������
    bool isBack;//�Ƿ�ʼ����

    public float minDistance;
    float keepTime;//�����Ҫ���ж������ֵ�ʱ��
    public float returnspeed;//�Զ����ص��ٶ�
    float maxTime;//������������ʱ������
    float startTime;//���ڼ�ʱ
    float expTime;//��������ʱ��
    public float speed;//δ�����ж�����ʱ�������ٶ�
    float changetime;//�ж����ƶ��ļ��ʱ��
    float moveStartTime;//�����ж����ƶ�
    float u;//�����ƶ��ж���ʱ���Բ�ֵ
    float cd = 0f;
    int n;//��ʾ����
    int m;//����ٶȵı仯
    int p;//�����ٶȵı仯
    int hit;//����
    float cd1 = 0f;
    float cd2 = 0f;
    float hitDis=0f;
    Animal thisAnimal;

    public float distance;//ָ������λ�Ʊ�����0-1��

    public int score;

    Vector3 start1Pos = new Vector3();//�ж���߽��ƶ�ʱ�Ŀ�ʼ������λ��
    Vector3 start2Pos = new Vector3();
    Vector3 end1Pos = new Vector3();
    Vector3 end2Pos = new Vector3();

    Vector3 poi = new Vector3(-9f * 0.8f, 6.3f * 0.8f - 1.2f, 0);
    Vector3 far = new Vector3(100f, 100f, 0);
    Vector3 slide_start1, slide_start2;//�ж����ʼλ��
    
    // Start is called before the first frame update
    void Start()
    {
        isFinishFirStep = false;
        isFinishSecStep = false;
        isEnter = false;
        isBulid = false;
        /*pos.transform.position = far;
        slide.SetActive(false);
        slide1.SetActive(false);
        slide2.SetActive(false);
        stripe.SetActive(false);*/
        isMoving = false;
        isOver = false;
        isBack = false;

        //������

        isFinishFirStep = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinishFirStep)
        {
            if (!isBulid)
            {
                //��ʼ����������
                bulid();
            }
            //��ʼִ���˶�����
            posMove();
            if (slide2.transform.position.x - slide1.transform.position.x < minDistance)
            {
                Vector3 min = slide1.transform.position;
                min.x = slide2.transform.position.x - minDistance;
                slide1.transform.position = min;
            }
                slideMove();
            if (slide2.transform.position.x - slide1.transform.position.x < minDistance)
            {
                Vector3 min = slide1.transform.position;
                min.x = slide2.transform.position.x - minDistance;
                slide1.transform.position = min;
            }
            if (isBulid)
            {
                slide.transform.position = (slide1.transform.position + slide2.transform.position) / 2;
                Vector3 size = new Vector3(1, 0.65f, 0);
                size.x = slide2.transform.position.x - slide1.transform.position.x;
                slide.transform.localScale = size;
            }
        }
        //�ж������������ܳ����߽�
        inTheLine(pos);
        inTheLine(slide1);
        inTheLine(slide2);

    }
    void bulid()
    {
        /*slide.SetActive(true);
        slide1.SetActive(true); 
        slide2.SetActive(true);
        stripe.SetActive(true);
        pos.transform.position = poi;*/
        isBulid = true;
        //
        thisAnimal = findTheNearest();
        //
        keepTime = thisAnimal.data.keepTime;
        maxTime = thisAnimal.data.maxTime;
        slide_start1 = thisAnimal.data.slideStart_L;
        slide_start2 = thisAnimal.data.slideStart_R;

        slide_start1.x *= 0.8f;
        slide_start1.y = slide_start1.y * 0.8f - 1.2f;
        slide_start2.x *= 0.8f;
        slide_start2.y = slide_start2.y * 0.8f - 1.2f;
        slide1.transform.position = slide_start1;
        slide2.transform.position = slide_start2;

        moveStartTime = 0f;

        hitDis = thisAnimal.data.hitDis;
        hit = 0;
        score = 0;
        startTime = 0;
        expTime = 0;
        isEnter = false;
        n = 0;
        m = 0;
        changetime = thisAnimal.data.change[0];
    }
    void posMove()//ָ����ƶ�
    {
        float horizon = Input.GetAxis("Horizontal");
        Vector3 nextPos = pos.transform.position;
        nextPos.x = nextPos.x + speed * horizon * Time.deltaTime + returnspeed * Time.deltaTime ;
        pos.transform.position = nextPos;
        if(isEnter&&m<thisAnimal.data.radSpeed.Length)
        {
            cd1 += Time.deltaTime;
            if(cd1>thisAnimal.data.radTime[m])
            {
                if (m > 0)
                    speed -= thisAnimal.data.radSpeed[m - 1];
                speed += thisAnimal.data.radSpeed[m];
                cd1 = 0f;
                m++;
            }
        }
        if (isEnter && p < thisAnimal.data.sceSpeed.Length)
        {
            cd2 += Time.deltaTime;
            if (cd2 > thisAnimal.data.sceTime[p])
            {
                if (p > 0)
                    returnspeed -= thisAnimal.data.sceSpeed[p - 1];
                returnspeed += thisAnimal.data.sceSpeed[p];
                cd2 = 0f;
                p++;
            }
        }
        if (pos.transform.position.x > slide_start1.x && !isEnter)
        {
            isEnter = true;
        }
        //Debug.Log(pos.transform.position.x + " " + slide1.gameObject.transform.position.x + " " + slide2.gameObject.transform.position.x);
        if (pos.transform.position.x > slide1.transform.position.x && pos.transform.position.x < slide2.transform.position.x && isEnter)
        {
            startTime += Time.deltaTime;
        }//����ڹ涨��Χ��ͼ�ʱ
        if (isEnter)
        {
            expTime += Time.deltaTime;
        }
        if (startTime >= keepTime)
            finishSecondStep();
        if (expTime >= maxTime)
        {
            fail();
            finishSecondStep();
        }
        distance = (pos.transform.position.x - (-9f)) / 18f;
    }
    void slideMove()//�ж�����ƶ�
    {
        if (slide2.transform.position.x - slide1.transform.position.x > minDistance + 0.00001f)
            if (pos.transform.position.x > slide2.transform.position.x)
            {
                if (cd < 0f)
                {
                    isOver = true;
                    cd =1f;
                   // Debug.Log("��ʹ");
                    hit++;
                }
                else
                {
                    cd -= Time.deltaTime;
                }

            }
        if (moveStartTime >= changetime&&!isMoving &&n<thisAnimal.data.change.Length)
        {
            u = 0f;
            if (n < thisAnimal.data.change.Length - 1)
                changetime = thisAnimal.data.change[n + 1];
            moveStartTime = 0;
            float firMove = thisAnimal.data.fir[n];
            float secMove = thisAnimal.data.sec[n];
            start1Pos = slide1.transform.position;
            start2Pos = slide2.transform.position;
            end1Pos = slide1.transform.position;
            end1Pos.x += firMove;
            end2Pos = slide2.transform.position;
            end2Pos.x += secMove;
            n++;
            if (slide2.transform.position.x - slide1.transform.position.x > minDistance + 0.00001f||firMove<secMove)
                isMoving = true;
           // Debug.Log("�����ƶ���λ��");
        }
        if (isEnter)
        {
            moveStartTime += Time.deltaTime;
        }
        if (isOver&&!isMoving)
        {
            float overMove1 = hitDis;
            start1Pos = slide1.transform.position;
            start2Pos = slide2.transform.position;
            end1Pos = slide1.transform.position;
            end1Pos.x += overMove1;
            end2Pos = slide2.transform.position;
            isOver = false;
            u = 0;
            isBack = true;
            //Debug.Log("ү����");
        }
        if (moveStartTime >= changetime && isMoving||isBack)
        {
            u += Time.deltaTime;
            //Debug.Log("�ƶ�"+u);
            slide1.transform.position = Vector3.Lerp(start1Pos, end1Pos, u/1.0f);
            slide2.transform.position = Vector3.Lerp(start2Pos, end2Pos, u/1.0f);
        if(u>1)
            {
                isMoving = false;
                isBack = false;
                moveStartTime = 0f;
                //�ƶ����и���ͻȻ�ı䷽��
                if (Random.Range(-1, 1) > 0)
                {
                    returnspeed *= -1;
                }
            }
        }
    }
    void finishSecondStep()
    {
        //��߷�����������������
        score = (int)((1 - (expTime - keepTime) / maxTime) * 100)-5*hit;
        Debug.Log("��ǰ����Ϊ"+score);
        //��ʼ��
        isFinishFirStep = false;
        
        //�������
        /*slide1.SetActive(false);
        slide2.SetActive(false);
        slide.SetActive(false);
        pos.transform.position = far;
        stripe.SetActive(false);*/

        isBulid = false;
        isFinishSecStep = true;
        LevelController.Instance.SwitchGameState(PlayerState.Transition);
    }
    void fail()
    {
        //Debug.Log("������");
    }
    void inTheLine(GameObject p)
    {
        if(p.transform.position.x>9*0.8f)
        {
            Vector3 pos = p.transform.position;
            pos.x = 9*0.8f;
            p.transform.position = pos;
        }
        if (p.transform.position.x < -9*0.8f)
        {
            Vector3 pos = p.transform.position;
            pos.x = -9*0.8f;
            p.transform.position = pos;
           // Debug.Log("Խ��"+p.transform.position);
        }
    }
    //Ѱ�Ҿ��������animal
    Animal findTheNearest()
    {
        Animal[] allAnimals = FindObjectsOfType<Animal>();
        Animal near = allAnimals[0];
        for(int i=1;i<allAnimals.Length;i++)
        {
            if(near.transform.position.x>allAnimals[i].transform.position.x)
            {
                near = allAnimals[i];
            }    
        }
        //Debug.Log("�ҵ��˶���" + near);
        return near;
    }
}
