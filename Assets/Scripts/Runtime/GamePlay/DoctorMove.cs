using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorMove : MonoBehaviour
{
    public L_or_R depthSource;
    public VerticalCheck heightSource;

    public Sprite testSprite;
    public Sprite turnAroundSprite;

    private float leftPos = -2.8f;
    private float rightPos = 0f;
    private float upPos = 1f;
    private float downPos = -1f;

    [SerializeField]
    public float moveSpeed = 6f;

    private float currentHeight;
    private float currentDepth;
    private Vector2 aimPos;

    private Transform armTransform;
    private Rigidbody2D handRigidbody;
    private Rigidbody2D arm1Rigidbody;
    private Rigidbody2D arm2Rigidbody;

    private void Awake()
    {
        armTransform = transform.Find("Arm");
        handRigidbody = transform.Find("Arm/Hand").GetComponent<Rigidbody2D>();
        arm1Rigidbody = transform.Find("Arm/UpperArm").GetComponent<Rigidbody2D>();
        arm2Rigidbody = transform.Find("Arm/LowerArm").GetComponent<Rigidbody2D>();
        Init();
    }

    public void Init()
    {
        currentHeight = currentDepth = 0f;
    }

    private void FixedUpdate()
    {
        if (heightSource != null) currentHeight = heightSource.CurrentHeight;
        if (depthSource != null)
        {
            if (depthSource.distance < 0.3f) currentDepth = depthSource.distance * 2;
            else currentDepth = 0.6f + (depthSource.distance - 0.3f) * 4f / 7f;
        }
        aimPos = new Vector2(Mathf.Lerp(leftPos, rightPos, currentDepth), Mathf.Lerp(downPos, upPos, currentHeight));
        handRigidbody.MovePosition(Vector2.MoveTowards(handRigidbody.position, aimPos, moveSpeed * Time.deltaTime));
        handRigidbody.MoveRotation(Quaternion.identity);
        handRigidbody.velocity = arm1Rigidbody.velocity = arm2Rigidbody.velocity = Vector2.zero;
        handRigidbody.angularVelocity = arm1Rigidbody.angularVelocity = arm2Rigidbody.angularVelocity = 0f;

        //Debug.Log(handRigidbody.position);
    }
}
