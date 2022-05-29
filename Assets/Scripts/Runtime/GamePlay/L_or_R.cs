using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_or_R : MonoBehaviour
{
    public GameObject stripe;//最下面的长条
    public GameObject slide;//控制区间的边界(左和右)
    public GameObject slide1, slide2;
    public GameObject pos;//指针

    public bool isFinishFirStep;//第一步是否完成(高度控制部分)
    public bool isTired;//是否处于疲劳状态
    public bool isFinishSecStep;
    bool isEnter;//是否进入控制区间(改变操作方式)
    bool isBulid;//控制UI显示
    bool isMoving;//控制判定框移动
    bool isOver;//是否戳到病人
    bool isBack;//是否开始后退

    public float minDistance;
    float keepTime;//玩家需要在判定区间坚持的时间
    public float returnspeed;//自动返回的速度
    float maxTime;//各个动物的最大时间限制
    float startTime;//用于计时
    float expTime;//经历的总时间
    public float speed;//未进入判定区间时操作的速度
    float changetime;//判定框移动的间隔时间
    float moveStartTime;//控制判定框移动
    float u;//用于移动判定框时线性差值
    float cd = 0f;
    int n;//表示波数
    int m;//随机速度的变化
    int p;//场景速度的变化
    int hit;//受伤
    float cd1 = 0f;
    float cd2 = 0f;
    float hitDis=0f;
    Animal thisAnimal;

    public float distance;//指针的相对位移比例（0-1）

    public int score;

    Vector3 start1Pos = new Vector3();//判定框边界移动时的开始，结束位置
    Vector3 start2Pos = new Vector3();
    Vector3 end1Pos = new Vector3();
    Vector3 end2Pos = new Vector3();

    Vector3 poi = new Vector3(-9f * 0.8f, 6.3f * 0.8f - 1.2f, 0);
    Vector3 far = new Vector3(100f, 100f, 0);
    Vector3 slide_start1, slide_start2;//判定框初始位置
    
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

        //测试用

        isFinishFirStep = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinishFirStep)
        {
            if (!isBulid)
            {
                //初始化几个部件
                bulid();
            }
            //开始执行运动函数
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
        //判定各个部件不能超过边界
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
    void posMove()//指针的移动
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
        }//如果在规定范围里就计时
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
    void slideMove()//判定框的移动
    {
        if (slide2.transform.position.x - slide1.transform.position.x > minDistance + 0.00001f)
            if (pos.transform.position.x > slide2.transform.position.x)
            {
                if (cd < 0f)
                {
                    isOver = true;
                    cd =1f;
                   // Debug.Log("啊痛");
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
           // Debug.Log("设置移动后位置");
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
            //Debug.Log("爷润了");
        }
        if (moveStartTime >= changetime && isMoving||isBack)
        {
            u += Time.deltaTime;
            //Debug.Log("移动"+u);
            slide1.transform.position = Vector3.Lerp(start1Pos, end1Pos, u/1.0f);
            slide2.transform.position = Vector3.Lerp(start2Pos, end2Pos, u/1.0f);
        if(u>1)
            {
                isMoving = false;
                isBack = false;
                moveStartTime = 0f;
                //移动后有概率突然改变方向
                if (Random.Range(-1, 1) > 0)
                {
                    returnspeed *= -1;
                }
            }
        }
    }
    void finishSecondStep()
    {
        //提高分数或者是其他反馈
        score = (int)((1 - (expTime - keepTime) / maxTime) * 100)-5*hit;
        Debug.Log("当前分数为"+score);
        //初始化
        isFinishFirStep = false;
        
        //清除场景
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
        //Debug.Log("输麻了");
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
           // Debug.Log("越界"+p.transform.position);
        }
    }
    //寻找距离最近的animal
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
        //Debug.Log("找到了动物" + near);
        return near;
    }
}
