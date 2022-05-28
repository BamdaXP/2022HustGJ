using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_or_R : MonoBehaviour
{
    public GameObject stripe;//������ĳ���
    public GameObject slide1,slide2;//��������ı߽�(�����)
    public GameObject pos;//ָ��

    public bool isFinishFirStep;//��һ���Ƿ����(�߶ȿ��Ʋ���)
    public bool isTired;//�Ƿ���ƣ��״̬
    bool isEnter;//�Ƿ�����������(�ı������ʽ)
    bool isBulid;//����UI��ʾ
    bool isMoving;//�����ж����ƶ�
    bool isOver;//�Ƿ��������

    public float change;//����ʱ�ƶ��ĳ���
    public float change_low;
    public float change_high;//������ʱ�����ֵ�������ƶ�����
    public float keepTime;//�����Ҫ���ж������ֵ�ʱ��
    public float returnspeed;//�Զ����ص��ٶ�
    public float maxTime;//������������ʱ������
    float startTime;//���ڼ�ʱ
    float expTime;//��������ʱ��
    public float speed;//δ�����ж�����ʱ�������ٶ�
    float changetime;//�ж����ƶ��ļ��ʱ��
    float moveStartTime;//�����ж����ƶ�
    float u;//�����ƶ��ж���ʱ���Բ�ֵ
    float i;//���ڼ����ƶ�
    int n;//��ʾ����
    Animal thisAnimal;

    int score;

    Vector3 start1Pos = new Vector3();//�ж���߽��ƶ�ʱ�Ŀ�ʼ������λ��
    Vector3 start2Pos = new Vector3();
    Vector3 end1Pos = new Vector3();
    Vector3 end2Pos = new Vector3();

    public Vector3 slide_start1, slide_start2;//�ж����ʼλ��
    
    // Start is called before the first frame update
    void Start()
    {
        isFinishFirStep = false;
        isEnter = false;
        isBulid = false;
        pos.SetActive(false);
        slide1.SetActive(false);
        slide2.SetActive(false);
        stripe.SetActive(false);
        isMoving = false;
        isOver = false;
        startTime = 0f;
        expTime = 0f;
        n = 0;
        thisAnimal = GetComponent<Animal>();
        score = 0;
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
            slideMove();  
        }
        //�ж������������ܳ����߽�
        inTheLine(pos);
        inTheLine(slide1);
        inTheLine(slide2);
    }
    void bulid()
    {
        slide1.SetActive(true);
        slide2.SetActive(true);
        slide1.transform.position = slide_start1;
        slide2.transform.position = slide_start2;
        pos.SetActive(true);
        stripe.SetActive(true);
        isBulid = true;
    }
    void posMove()//ָ����ƶ�
    {
        if(isTired)
        {
            change = Random.Range(change_low, change_high);
        }
        if(pos.transform.position.x>-9)
        {
            Vector3 nowPos = pos.transform.position;
            nowPos.x -= returnspeed * Time.deltaTime;
            pos.transform.position = nowPos;
        }
        if(Input.GetKey(KeyCode.RightArrow)&&!isEnter)
        {
            //Debug.Log("���¿ո�");
            Vector3 nowPos = pos.transform.position;
            i += Time.deltaTime;
            nowPos.x += i * speed * Time.deltaTime;
            pos.transform.position = nowPos;
        }//�ڽ��뱣�ַ�Χǰ
        if (!Input.GetKey(KeyCode.RightArrow))
            i = 1f;
        if (pos.transform.position.x > (slide_start1.x + slide_start2.x) / 2 && pos.transform.position.x > 0 && !isEnter)
        {
            isEnter = true;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)&&isEnter)
        {
            Vector3 nowPos = pos.transform.position;
            nowPos.x -= change;
            pos.transform.position = nowPos;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && isEnter)
        {
            Vector3 nowPos = pos.transform.position;
            nowPos.x += change;
            //Debug.Log(change);
            pos.transform.position = nowPos;
        }
        //Debug.Log(pos.transform.position.x + " " + slide1.gameObject.transform.position.x + " " + slide2.gameObject.transform.position.x);
        if (pos.transform.position.x > slide1.transform.position.x && pos.transform.position.x < slide2.transform.position.x && isEnter)
        {
            startTime += Time.deltaTime;
        }//����ڹ涨��Χ��ͼ�ʱ
        expTime += Time.deltaTime;
        if (startTime >= keepTime)
            finishSecondStep();
        if (expTime >= maxTime)
        {
            fail();
            finishSecondStep();
        }
    }
    void slideMove()//�ж�����ƶ�
    {
        if(pos.transform.position.x>slide2.transform.position.x)
        {
            isOver = true;
            //Debug.Log("��ʹ");
        }
        if (!isMoving&&n<thisAnimal.data.lev.Length)
        {
            u = 0f;
            changetime = thisAnimal.data.lev[n].changeDuration;
            moveStartTime = 0;
            float firMove = thisAnimal.data.lev[n].firMove;
            float secMove = thisAnimal.data.lev[n].secMove;
            start1Pos = slide1.transform.position;
            start2Pos = slide2.transform.position;
            end1Pos = slide1.transform.position;
            end1Pos.x += firMove;
            end2Pos = slide2.transform.position;
            end2Pos.x += secMove;
            n++;
            if (isOver)
            {
                float overMove1 = Random.Range(0.4f, 1f);
                float overMove2 = Random.Range(0f, 0.6f); 
                end1Pos.x += overMove1;
                end2Pos.x += overMove2;
                isOver = false;
                //Debug.Log("ү����");
            }
               
            isMoving = true;
           // Debug.Log("�����ƶ���λ��");
        }
        if (isEnter)
        {
            moveStartTime += Time.deltaTime;
        }
        if (moveStartTime >= changetime && isMoving)
        {
            u += Time.deltaTime;
            //Debug.Log("�ƶ�"+u);
            slide1.transform.position = Vector3.Lerp(start1Pos, end1Pos, u/1.0f);
            slide2.transform.position = Vector3.Lerp(start2Pos, end2Pos, u/1.0f);
        if(u>1)
            {
                isMoving = false;
                //�ƶ����и���ͻȻ�ı䷽��
                if(Random.Range(-1,1)>0)
                {
                    returnspeed = Random.Range(1, 3);
                }
                else
                {
                    returnspeed = Random.Range(-1, -3);
                }
            }
        }
    }
    void finishSecondStep()
    {
        //��߷�����������������
        score += (int)(1 - (expTime - keepTime) / maxTime) * 100;
        //��ʼ��
        startTime = 0;
        expTime = 0;
        isEnter = false;
        isFinishFirStep = false;
        
        //�������
        slide1.SetActive(false);
        slide2.SetActive(false);
        pos.SetActive(false);
        stripe.SetActive(false);
        isBulid = false;
    }
    void fail()
    {
        Debug.Log("������");
    }
    void inTheLine(GameObject p)
    {
        if(p.transform.position.x>9)
        {
            Vector3 pos = p.transform.position;
            pos.x = 9;
            p.transform.position = pos;
        }
        if (p.transform.position.x < -9)
        {
            Vector3 pos = p.transform.position;
            pos.x = -9;
            p.transform.position = pos;
        }
    }
}
