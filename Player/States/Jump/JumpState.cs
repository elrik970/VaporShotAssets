using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "JumpState", menuName = "States/Player/JumpState")]
public class JumpState : PlayerState<Player>
{
    public float jumpGravityScale;
    public float variableJumpHeightMultiplier;
    private bool onceLifted = false;

    public override void Init(Player parent) {
        base.Init(parent);
        player.SetGravity(jumpGravityScale);

        Player.Inputs.Default.Jump.canceled += OnJumpRelease;
        Player.Inputs.Default.Dash.performed += OnDash;

        onceLifted = false;

        player.Forces.curCoyoteTime = player.Forces.coyoteTime;



    }
    public override void ConstantUpdate() {
        
    }
    public override void CaptureInputs() {

    }
    public override void ChangeState() {
        if (player.OnWall(player.Char.Body.right)) {
            runner.SetState(player.States.WallRunningRightState);
        }
        if (player.OnWall(-player.Char.Body.right)) {
            runner.SetState(player.States.WallRunningLeftState);
        }
    }
    public override void PhysicsUpdate() {
        if (!player.OnGround()) {
            onceLifted = true;
        }
        player.Move(Player.Inputs.Default.Movement.ReadValue<Vector2>());
        if (rb.velocity.y < 0f) {
            runner.SetState(player.States.FallingState);
        }
        if (onceLifted&&player.OnGround()) {
            runner.SetState(player.States.IdleState);
        }
    }
    public override void Exit() {
        Player.Inputs.Default.Jump.canceled -= OnJumpRelease;
        Player.Inputs.Default.Dash.performed -= OnDash;
        player.SetGravity(player.Forces.defaultGravityScale);
    }

    void OnJumpRelease(InputAction.CallbackContext context) {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y*variableJumpHeightMultiplier,rb.velocity.z);
    }
    void OnDash(InputAction.CallbackContext context) {
        player.Dash();
    }
}
