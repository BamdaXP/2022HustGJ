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
    [SerializeField]
    private float m_height = 0.5f;
    [SerializeField]
    private float[] m_heightRange = { 0.14f, 0.16f, 0.2f, 0.24f };
    [SerializeField]
    private string[] m_judgeWord = { "Perfect", "Excellent", "Great", "Good" };
    [SerializeField]
    private Transform[] m_heightRangeTransform;
    private float[] m_upperLimit;
    private float[] m_lowerLimit;

    private Transform upperBound;
    private Transform lowerBound;

    private float m_velocity;
    [SerializeField]
    private float m_keyMaxVelocity = 50f;
    [SerializeField]
    private float m_mouseMaxVelocity = 1000f;
    private float m_aimVelocity;
    [SerializeField]
    private float m_accelaration = 100f;
    [SerializeField]
    private float m_decelaration = 10f;

    private bool m_roundEnd;
    private bool m_finished;
    private bool m_canControl;
    private float m_pressTimer;
    private float m_maxPressTime = 2f;
    [SerializeField]
    private Slider timeSlider;

    private void Start()
    {
        SetCheckArea();
        upperBound = transform.Find("UpperBound");
        lowerBound = transform.Find("LowerBound");
        m_velocity = m_aimVelocity = 0f;
        m_finished = false;
        m_canControl = true;
    }

    private void Update()
    {
        if (!m_finished)
        {
            MoveAndCheck();
        }
    }

    private float scaleProp = 50f;
    public void SetCheckArea()
    {
        m_upperLimit = new float[m_heightRange.Length];
        m_lowerLimit = new float[m_heightRange.Length];
        for (int i = 0; i < m_heightRange.Length; i++)
        {
            m_upperLimit[i] = Mathf.Lerp(m_maxLowerLimit, m_maxUpperLimit, m_height + m_heightRange[i] / 2);
            m_lowerLimit[i] = Mathf.Lerp(m_maxLowerLimit, m_maxUpperLimit, m_height - m_heightRange[i] / 2);
            m_heightRangeTransform[i].position = new Vector2(m_heightRangeTransform[i].position.x, Mathf.Lerp(m_maxLowerLimit, m_maxUpperLimit, m_height));
            m_heightRangeTransform[i].localScale = new Vector2(m_heightRangeTransform[i].localScale.x, m_heightRange[i] * scaleProp);
        }
    }

    private void MoveAndCheck()
    {
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
        timeSlider.value = m_pressTimer / m_maxPressTime;
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
                return;
            }
            else if (!m_canControl && !m_roundEnd)
            {
                Debug.Log("EndRound");
                m_roundEnd = true;
                transform.DOMoveY(m_maxLowerCenter, 0.5f);
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
        if (lowerBound.position.y < m_lowerLimit[m_upperLimit.Length - 1] ||
            upperBound.position.y > m_upperLimit[m_lowerLimit.Length - 1])
        {
            return false;
        }
        else
        {
            Debug.Log("In!");
            for (int i = 0; i < m_heightRange.Length; i++)
            {
                if (upperBound.position.y <= m_upperLimit[i] && 
                    lowerBound.position.y >= m_lowerLimit[i])
                {
                    Debug.Log(m_judgeWord[i]);
                    break;
                }
            }
            return true;
        }
    }
}
