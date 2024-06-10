using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    public float time;
    private float curTime = 0f;
    public Rigidbody rb;
    public Rigidbody playerRb;
    private bool Moving = false;
    public Vector3 MoveDirection;
    private Vector3 lastPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Moving) {
            curTime += Time.deltaTime;
            
            Vector3 curPos = transform.position;
            playerRb.transform.position+=curPos-lastPos;

            
            
            if (curTime > time) {
                rb.velocity = Vector3.zero;
                curTime = 0f;
                Moving = false;
            }

            lastPos = transform.position;
        }
    }
    void OnCollisionEnter(Collision col) {
        
        if (col.gameObject.tag == "Player") {
            Moving = true;
            rb.velocity = MoveDirection;
            playerRb = col.gameObject.GetComponent<Rigidbody>();
            lastPos = playerRb.transform.position;
        }
    }
}
