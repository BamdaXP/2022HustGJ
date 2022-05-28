using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointTemp : MonoBehaviour
{
    private void Start()
    {
        //GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100f);
        //GetComponent<Rigidbody2D>().angularVelocity = 100f;
        //GetComponent<Rigidbody2D>().velocity = Vector2.up * 10f;
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + Vector2.up * 1f * Time.deltaTime);
    }
}
