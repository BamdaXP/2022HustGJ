using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_or_R : MonoBehaviour
{
    public GameObject stripe;//最下面的长条
    public GameObject slide1,slide2;//控制区间的边界(左和右)
    public GameObject pos;//指针

    public bool isFinishFirStep;//第一步是否完成(高度控制部分)
    public bool isTired;//是否处于疲劳状态
    bool isEnter;//是否进入控制区间(改变操作方式)
    bool isBulid;//控制UI显示
    bool isMoving;//控制判定框移动
    bool isOver;//是否戳到病人

    public float change;//单点时移动的长度
    public float change_low;
    public float change_high;//在劳累时用随机值来控制移动长度
    public float keepTime;//玩家需要在判定区间坚持的时间
    public float returnspeed;//自动返回的速度
    public float maxTime;//各个动物的最大时间限制
    float startTime;//用于计时
    float expTime;//经历的总时间
    public float speed;//未进入判定区间时操作的速度
    float changetime;//判定框移动的间隔时间
    float moveStartTime;//控制判定框移动
    float u;//用于移动判定框时线性差值
    float i;//用于加速移动
    int n;//表示波数
    Animal thisAnimal;

    int score;

    Vector3 start1Pos = new Vector3();//判定框边界移动时的开始，结束位置
    Vector3 start2Pos = new Vector3();
    Vector3 end1Pos = new Vector3();
    Vector3 end2Pos = new Vector3();

    public Vector3 slide_start1, slide_start2;//判定框初始位置
    
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
                //初始化几个部件
                bulid();
            }
            //开始执行运动函数
            posMove();
            slideMove();  
        }
        //判定各个部件不能超过边界
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
    void posMove()//指针的移动
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
            //Debug.Log("按下空格");
            Vector3 nowPos = pos.transform.position;
            i += Time.deltaTime;
            nowPos.x += i * speed * Time.deltaTime;
            pos.transform.position = nowPos;
        }//在进入保持范围前
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
        }//如果在规定范围里就计时
        expTime += Time.deltaTime;
        if (startTime >= keepTime)
            finishSecondStep();
        if (expTime >= maxTime)
        {
            fail();
            finishSecondStep();
        }
    }
    void slideMove()//判定框的移动
    {
        if(pos.transform.position.x>slide2.transform.position.x)
        {
            isOver = true;
            //Debug.Log("啊痛");
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
                //Debug.Log("爷润了");
            }
               
            isMoving = true;
           // Debug.Log("设置移动后位置");
        }
        if (isEnter)
        {
            moveStartTime += Time.deltaTime;
        }
        if (moveStartTime >= changetime && isMoving)
        {
            u += Time.deltaTime;
            //Debug.Log("移动"+u);
            slide1.transform.position = Vector3.Lerp(start1Pos, end1Pos, u/1.0f);
            slide2.transform.position = Vector3.Lerp(start2Pos, end2Pos, u/1.0f);
        if(u>1)
            {
                isMoving = false;
                //移动后有概率突然改变方向
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
        //提高分数或者是其他反馈
        score += (int)(1 - (expTime - keepTime) / maxTime) * 100;
        //初始化
        startTime = 0;
        expTime = 0;
        isEnter = false;
        isFinishFirStep = false;
        
        //清除场景
        slide1.SetActive(false);
        slide2.SetActive(false);
        pos.SetActive(false);
        stripe.SetActive(false);
        isBulid = false;
    }
    void fail()
    {
        Debug.Log("输麻了");
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
