using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class VerticalCheck : MonoBehaviour
{
    private float m_maxUpperLimit = 4f;
    private float m_maxLowerLimit = -4f;
    private float m_maxUpperCenter = 3.6f;
    private float m_maxLowerCenter = -3.6f;
    private Transform upperBound;
    private Transform lowerBound;

    [SerializeField]
    private float m_height = 0.5f;
    [SerializeField]
    private float m_heightRange = 0.2f;
    [SerializeField]
    private Transform m_heightRangeTransform;
    private float m_upperLimit;
    private float m_lowerLimit;

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
    private bool m_finished;
    [SerializeField]
    private float m_maxPressTime = 1f;
    [SerializeField]
    private Slider m_timeSlider;

    private void Start()
    {
        upperBound = transform.Find("UpperBound");
        lowerBound = transform.Find("LowerBound");
        Init();
    }

    private float scaleProp = 50f;
    public void Init()
    {
        m_upperLimit = Mathf.Lerp(m_maxLowerLimit, m_maxUpperLimit, m_height + m_heightRange / 2);
        m_lowerLimit = Mathf.Lerp(m_maxLowerLimit, m_maxUpperLimit, m_height - m_heightRange / 2);
        m_heightRangeTransform.position = new Vector2(m_heightRangeTransform.position.x, Mathf.Lerp(m_maxLowerLimit, m_maxUpperLimit, m_height));
        m_heightRangeTransform.localScale = new Vector2(m_heightRangeTransform.localScale.x, m_heightRange * scaleProp);
        m_velocity = m_aimVelocity = 0f;
        m_roundEnd = false;
        m_finished = false;
        m_canControl = true;
        m_pressTimer = 0f;
    }

    private void Update()
    {
        if (!m_finished)
        {
            MoveAndCheck();
        }
    }

    private void MoveAndCheck()
    {
        m_timeSlider.value = m_pressTimer / m_maxPressTime;
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
            Debug.Log("Mouse Y : " + Input.GetAxisRaw("Mouse Y"));
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
                transform.DOMoveY(m_maxLowerCenter, 0.5f);
                DOTween.To(() => m_pressTimer, x => m_pressTimer = x, 0, 0.5f);
                Invoke("CanControl", 0.5f);
            }
        }
        float aimY = transform.position.y + m_velocity * Time.deltaTime;
        if (aimY > m_maxUpperCenter)
        {
            aimY = m_maxUpperCenter;
            m_velocity = m_aimVelocity = 0f;
        }
        else if (aimY < m_maxLowerCenter)
        {
            aimY = m_maxLowerCenter;
            m_velocity = m_aimVelocity = 0f;
        }
        transform.position = new Vector2(transform.position.x, aimY);
    }

    private void CanControl()
    {
        m_canControl = true;
        m_roundEnd = false;
    }

    private bool IsInRange()
    {
        return upperBound.position.y <= m_upperLimit && lowerBound.position.y >= m_lowerLimit;
    }
}