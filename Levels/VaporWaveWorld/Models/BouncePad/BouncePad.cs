using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float BounceForce;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Player")) {

            Rigidbody rb = col.transform.parent.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
            rb.AddForce(Vector3.up*BounceForce, ForceMode.Impulse);
            Player player = col.transform.parent.GetComponent<Player>();
            col.transform.parent.GetComponent<StateManager>().SetState(player.States.RisingState);
            player.Forces.canDash = true;
        }
    }
}
