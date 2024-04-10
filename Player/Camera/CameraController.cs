using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Player Player;
    void Start()
    {
        Player = GetComponent<Player>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 mouseDelta = Player.Inputs.Default.MoveHead.ReadValue<Vector2>();

        Vector2 RotateVector = mouseDelta*Player.HeadControls.sensitivity/1000f;

        Player.HeadControls.Head.localRotation *= Quaternion.Euler(-RotateVector.y,0f,0f);


        float xRotation = Player.HeadControls.Head.localEulerAngles.x;

        

        if (xRotation > 200) {
            xRotation = 0f-(360f-xRotation);
        }

        xRotation = Mathf.Clamp(xRotation,-85f,85f);        

        Player.HeadControls.Head.localEulerAngles = new Vector3(xRotation,0f,Player.HeadControls.zAngle);

        Player.Char.Body.rotation *= Quaternion.Euler(0f,RotateVector.x,0f);
        // Player.Char.Body.localEulerAngles = new Vector3(0f,Player.transform.eulerAngles.y+Time.deltaTime*100f,0f);
    }

}
