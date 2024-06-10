using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public StateManager runner;
    public static PlayerInputs Inputs;
    public HeadControls HeadControls;
    public States States;
    public Forces Forces;
    public Character Char;
    public ParticleFX ParticleFX;
    
    


    void Awake() {
        Inputs = new PlayerInputs();
    }
    void OnEnable() {
        Inputs.Enable();
    }
    void OnDisable() {
        if (Inputs != null) {
            Inputs.Disable();
        }
        Cursor.lockState = CursorLockMode.None;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        runner = GetComponent<StateManager>();
        
        Forces.constantForce = GetComponent<ConstantForce>();
        Inputs.Default.Jump.performed += OnJump;
    }

    // Update is called once per frame
    void Update()
    {
        Forces.curDashTime += Time.deltaTime;
        if (OnGround()) {
            Forces.canDash = true;
        }
        Forces.curCoyoteTime += Time.deltaTime;
        if (Forces.curCoyoteTime > Forces.coyoteTime) {
            Forces.curCoyoteTime = Forces.coyoteTime;
        }
        if (!OnGround()) {
            Forces.MoveReference.transform.localRotation = Quaternion.identity;
        }

        Forces.curWallCoolDown += Time.deltaTime;

        if (Forces.curWallCoolDown > Forces.maxWallCoolDown) {
            Forces.curWallCoolDown = Forces.maxWallCoolDown;
        }
        
    }
    public bool OnWall(Vector3 direction) {
        RaycastHit hit;
        Debug.DrawRay(transform.position, direction * Char.width*Forces.wallRunCheckSize, Color.red);
        if (Forces.curWallCoolDown >= Forces.maxWallCoolDown && Physics.Raycast(transform.position, direction, out hit, Char.width*Forces.wallRunCheckSize, Char.GroundLayerMask)) {
            return true;
        }
        else {
            return false;
        }
    }
    public void Jump(float jumpForce = 35f) {
        rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        rb.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
        Forces.MoveReference.transform.localRotation = Quaternion.identity;
    }
    public void SetGravity(float gravityScale) {
        Forces.constantForce.force = new Vector3(0f,-gravityScale,0f);
    }
    public bool OnGround() {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, Char.width, Vector3.down, out hit, Char.halfHeight, Char.GroundLayerMask)) {
            return true;
        }
        return false;
    }
    public RaycastHit GroundRay() {
        RaycastHit hit;

        Physics.SphereCast(transform.position, Char.width, Vector3.down, out hit, Char.halfHeight, Char.GroundLayerMask);
        return hit;
    }
    public void Move(Vector2 MoveDirection,bool sprinting = false,bool grounded = false) {
        sprinting = true;

        Vector3 nonYvelocity = Vector3.zero;
        if (!grounded) {
            nonYvelocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        }
        else {
            nonYvelocity = new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z);
        }
        TrailEffect();
        SpeedEffect();
        
        if (sprinting&&MoveDirection.y > 0f) {
            MoveDirection = new Vector2(MoveDirection.x,MoveDirection.y*Forces.sprintMultiplier);
        }

        Vector3 worldSpaceMoveDirection = Forces.MoveReference.transform.rotation * new Vector3(MoveDirection.x,0f,MoveDirection.y);

        Vector3 newDirection = new Vector3(worldSpaceMoveDirection.x*Forces.moveSpeed,0f,worldSpaceMoveDirection.z*Forces.moveSpeed)-nonYvelocity;

        if (!grounded) {
            if (Vector3.Dot(newDirection.normalized,worldSpaceMoveDirection.normalized) < 0f) {
                worldSpaceMoveDirection = worldSpaceMoveDirection.normalized*nonYvelocity.magnitude;
                newDirection = worldSpaceMoveDirection-nonYvelocity;
            }
        }
        rb.AddForce(newDirection*Forces.acceleration,ForceMode.Impulse);
    }
    public void SlideMove(Vector2 MoveDirection,bool sprinting, float speed, float acceleration) {
        sprinting = true;

        Vector3 nonYvelocity = Vector3.zero;
        
        nonYvelocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);

        TrailEffect();
        SpeedEffect();
        
        if (sprinting&&MoveDirection.y > 0f) {
            MoveDirection = new Vector2(MoveDirection.x,MoveDirection.y*Forces.sprintMultiplier);
        }

        Vector3 worldSpaceMoveDirection = Forces.MoveReference.transform.rotation * new Vector3(MoveDirection.x,0f,MoveDirection.y);

        Vector3 newDirection = new Vector3(worldSpaceMoveDirection.x*speed*Forces.moveSpeed,0f,worldSpaceMoveDirection.z*speed*Forces.moveSpeed)-nonYvelocity;


        if (Vector3.Dot(newDirection.normalized,worldSpaceMoveDirection.normalized) < 0f) {
            worldSpaceMoveDirection = worldSpaceMoveDirection.normalized*nonYvelocity.magnitude;
            newDirection = worldSpaceMoveDirection-nonYvelocity;
        }
        rb.AddForce(newDirection*acceleration,ForceMode.Impulse);
    }
    
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Kill") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void SpeedEffect() {
        bool grounded = OnGround();
        Vector3 nonYvelocity = Vector3.zero;
        if (!grounded) {
            nonYvelocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);

        }
        else {
            nonYvelocity = new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z);
        }
        
        if (nonYvelocity.sqrMagnitude > ParticleFX.minSpeedEffectSpeed) {
            ParticleFX.speedEffect.Play();
        }
        else {
            ParticleFX.speedEffect.Stop();
        }
    }
    void TrailEffect() {
        if (OnGround()) {
            ParticleFX.trailEffect.Play();
        }
        else {
            ParticleFX.trailEffect.Stop();
        }
    }
    public void SlopeCheck() {
        if (OnGround()) {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, Char.width, Vector3.down, out hit, Char.halfHeight,Char.GroundLayerMask)) {
                Forces.MoveReference.transform.localRotation = Quaternion.FromToRotation(transform.up,hit.normal);
                if (hit.normal != Vector3.up) {
                    SetGravity(0f);
                }
            }
            
        }
        
        
    }
    private void OnJump(InputAction.CallbackContext context) {
        if (Forces.curCoyoteTime < Forces.coyoteTime) {
            Jump();
            runner.SetState(States.JumpState);
            Forces.curCoyoteTime = Forces.coyoteTime;
        }
        
    }
    public void Dash() {
        if (Forces.canDash&&Forces.curDashTime > Forces.DashCoolDown) {
            runner.SetState(States.DashState);
        }
        
    }
    public void DoubleJumpFx() {
        foreach (ParticleSystem ps in ParticleFX.DoubleJump) {
            ps.Play();
        }
    }
}
[System.Serializable]
public struct HeadControls {
    public float sensitivity;
    public Transform Head;
    public float zAngle;
}
[System.Serializable]
public struct States {
    public PlayerState<Player> IdleState;
    public PlayerState<Player> SlideState;
    public PlayerState<Player> RisingState;
    public PlayerState<Player> FallingState;
    public PlayerState<Player> JumpState;
    public PlayerState<Player> DashState;
    public PlayerState<Player> WallRunningRightState;
    public PlayerState<Player> WallRunningLeftState;
}
[System.Serializable]
public struct Forces {
    public float defaultGravityScale;
    public float jumpForce;
    public float coyoteTime;
    public float curCoyoteTime;
    public ConstantForce constantForce;
    public float moveSpeed;
    public float acceleration;
    public float sprintMultiplier;
    public Transform MoveReference;
    public bool wallRunning;
    public float wallRunCheckSize;
    public float curWallCoolDown;
    public float maxWallCoolDown;
    public bool canDash;
    public float curDashTime;
    public float DashCoolDown;
}
[System.Serializable]
public struct Character {
    public float width;
    public float halfHeight;
    public LayerMask GroundLayerMask;
    public Transform Body;
    public Animator anim;
}
[System.Serializable]
public struct ParticleFX {
    public Image Cursor;
    public Color defaultCursorColor;
    public ParticleSystem speedEffect;
    public Color defaultSpeedEffectColor;
    public float minSpeedEffectSpeed;
    public ParticleSystem trailEffect;
    public VolumeProfile DefaultVolume;
    public Volume GlobalVolume;
    public ParticleSystem[] DoubleJump;
}



