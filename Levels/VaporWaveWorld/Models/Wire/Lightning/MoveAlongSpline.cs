using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
public class MoveAlongSpline : MonoBehaviour
{
    // Start is called before the first frame update
    public SplineContainer spline;
    private float curTime;
    private float timeMult = 1;
    void Start()
    {
        spline = transform.parent.GetComponent<SplineContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        curTime+=timeMult*Time.deltaTime;
        Vector3 curPos = spline.EvaluatePosition(curTime);
        transform.position = curPos;

        if (curTime > 1f) {
            timeMult = -1f;
        }
        if (curTime < 0f) {
            timeMult = 1f;
        }
    }
}
