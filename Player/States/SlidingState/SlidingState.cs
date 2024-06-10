using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SlidingState", menuName = "States/Player/SlidingState")]
public class SlidingState : PlayerState<Player>
{
    // Start is called before the first frame update
    public float slidingGravity = 50;
    public float slidingAccel = 0.05f;
    public float slidingMultiplier = 1.3f;
    private float curSlidingMult;
    public float Drag = 0.1f;
    public float timeOffGround;
    private float curTimeOffGround;
    public override void Init(Player parent) {
        base.Init(parent);
        player.SetGravity(slidingGravity);
        player.Char.anim.Play("Slide",-1,0f);
        Player.Inputs.Default.Jump.performed += OnJump;

        player.Forces.curCoyoteTime = player.Forces.coyoteTime;
        Player.Inputs.Default.Dash.performed += OnDash;
        curTimeOffGround = 0f;

    }
    public override void ConstantUpdate() {
    }
    public override void CaptureInputs() {

    }
    public override void ChangeState() {
        if (Player.Inputs.Default.Slide.ReadValue<float>() == 0f) {
            runner.SetState(player.States.IdleState);
        }
    }
    public override void PhysicsUpdate() {
        
        if (!player.OnGround()) {
            curTimeOffGround+=Time.deltaTime;
            if (curTimeOffGround > timeOffGround) {
                runner.SetState(player.States.FallingState);
            }
        }
        else {
            curTimeOffGround = 0f;
        }
        player.SlideMove(Player.Inputs.Default.Movement.ReadValue<Vector2>(),Player.Inputs.Default.Sprint.ReadValue<float>() > 0,slidingMultiplier,slidingAccel);
    }
    public override void Exit() {
        Player.Inputs.Default.Jump.performed -= OnJump;
        player.Char.anim.Play("SlideExit",-1,0f);
        player.SetGravity(player.Forces.defaultGravityScale);
        player.Forces.curCoyoteTime = 0f;
        Player.Inputs.Default.Dash.performed -= OnDash;
    }
    void OnJump(InputAction.CallbackContext context) {
        player.Jump();
        runner.SetState(player.States.JumpState);
    }
    void OnDash(InputAction.CallbackContext context) {
        player.Dash();
    }
}
