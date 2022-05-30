using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Test : MonoBehaviour
{
    public Transform point;
    [Range(0f,1f)]
    public float pointPercentage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        point.transform.localPosition = new Vector3(GetX(pointPercentage), point.transform.localPosition.y, point.transform.localPosition.z);
    }

    public float GetX(float percentage)
    {
        return (transform as RectTransform).rect.width * (percentage-0.5f);
    }
}
