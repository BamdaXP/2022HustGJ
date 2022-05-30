using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Stage1 : MonoBehaviour
{
    [SerializeField]
    private Transform m_redFillTransform;
    [SerializeField]
    private Transform m_checkAreaTransform;

    private float m_currentHeight;
    [SerializeField]
    private float m_height = 0.5f;
    [SerializeField]
    private float m_heightRange = 0.2f;

    private float m_velocity;
    //[SerializeField]
    //private float m_keyMaxVelocity = 50f;
    [SerializeField]
    private float m_mouseMaxVelocity = 100f;
    private float m_aimVelocity;
    [SerializeField]
    private float m_accelaration = 100f;
    [SerializeField]
    private float m_decelaration = 10f;

    private bool m_roundEnd;
    private bool m_canControl;
    private float m_pressTimer;
    [SerializeField]
    private bool m_finished;
    [SerializeField]
    private float m_maxPressTime = 1f;
    [SerializeField]
    private Slider m_timeSlider;
    private int m_score;

    public float CurrentHeight
    {
        get { return m_currentHeight; }
    }

    public bool Finished
    {
        get { return m_finished; }
    }

    public int Score
    {
        get { return m_score; }
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        m_checkAreaTransform.localPosition = new Vector2
            (m_checkAreaTransform.localPosition.x, m_height * m_heightMultiplier * 2);
        m_checkAreaTransform.localScale = new Vector2
            (m_checkAreaTransform.localScale.x, m_heightRange * m_scaleMultiplier);
        m_currentHeight = 0f;
        m_velocity = m_aimVelocity = 0f;
        m_roundEnd = false;
        m_finished = false;
        m_canControl = true;
        m_pressTimer = 0f;
        m_score = 0;
    }

    private void Update()
    {
        if (LevelController.Instance.PlayerState == PlayerState.Stage1 && !m_finished)
        {
            MoveAndCheck();
        }
    }

    private void MoveAndCheck()
    {
        m_timeSlider.value = 1 - m_pressTimer / m_maxPressTime;
        m_redFillTransform.localScale = new Vector3(m_redFillTransform.localScale.x, m_currentHeight, m_redFillTransform.localScale.z);

        if (m_roundEnd) return;

        // МќХЬ
        //m_aimVelocity = Input.GetAxisRaw("Vertical") * m_keyMaxVelocity;

        // ЪѓБъ
        if (Input.GetMouseButtonDown(0) && m_canControl)
        {
            m_pressTimer = 0f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_aimVelocity = 0f;
            m_canControl = false;
        }
        else if (Input.GetMouseButton(0) && m_canControl)
        {
            m_aimVelocity = Input.GetAxisRaw("Mouse Y") * m_mouseMaxVelocity;
            //Debug.Log("Mouse Y : " + Input.GetAxisRaw("Mouse Y"));
            //Debug.Log("Aim Velocity : " + m_aimVelocity);
            m_pressTimer += Time.deltaTime;
            if (m_pressTimer > m_maxPressTime)
            {
                m_aimVelocity = 0f;
                m_canControl = false;
            }
        }
        if ((m_velocity < 0 && m_aimVelocity > 0) || (m_velocity > 0 && m_aimVelocity < 0) || 
            Mathf.Abs(m_velocity) < Mathf.Abs(m_aimVelocity))
        {
            m_velocity = Mathf.MoveTowards(m_velocity, m_aimVelocity, m_accelaration * Time.deltaTime);
        }
        else
        {
            m_velocity = Mathf.MoveTowards(m_velocity, m_aimVelocity, m_decelaration * Time.deltaTime);
        }

        if (Mathf.Approximately(m_velocity, 0f))
        {
            if (IsInRange())
            {
                m_finished = true;
                Debug.Log("Excellent!");
                return;
            }
            else if (!m_canControl && !m_roundEnd)
            {
                Debug.Log("EndRound");
                m_roundEnd = true;
                DOTween.To(() => m_pressTimer, x => m_pressTimer = x, 0, 0.5f);
                DOTween.To(() => m_currentHeight, x => m_currentHeight = x, 0, 0.5f);
                Invoke("CanControl", 0.5f);
            }
        }
        m_currentHeight += m_velocity * Time.deltaTime;
        if (m_currentHeight > 1f)
        {
            m_currentHeight = 1f;
            m_velocity = m_aimVelocity = 0f;
        }
        else if (m_currentHeight < 0f)
        {
            m_currentHeight = 0;
            m_velocity = m_aimVelocity = 0f;
        }
    }

    private void CanControl()
    {
        m_canControl = true;
        m_roundEnd = false;
    }

    private bool IsInRange()
    {
        if (m_currentHeight > m_height + m_heightRange / 2 || m_currentHeight < m_height - m_heightRange / 2)
            return false;
        m_score = 100 - (int)((m_currentHeight - m_height) / m_heightRange * 50);
        return true;
    }

    private float m_heightMultiplier = 5.2f;
    private float m_scaleMultiplier = 6.2f;
}