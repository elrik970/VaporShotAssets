using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Grounded", menuName = "States/Player/Grounded")]
public class Grounded : PlayerState<Player>
{
    public override void Init(Player parent) {
        base.Init(parent);
        Player.Inputs.Default.Jump.performed += OnJump;

        player.Forces.curCoyoteTime = player.Forces.coyoteTime;
        Player.Inputs.Default.Dash.performed += OnDash;

        


    }
    public override void ConstantUpdate() {
        player.SlopeCheck();
    }  
    public override void CaptureInputs() {
        
    }
    public override void ChangeState() {
        if (player.OnWall(player.Char.Body.right)) {
            runner.SetState(player.States.WallRunningRightState);
        }
        if (!player.OnGround()) {
            runner.SetState(player.States.FallingState);
        }

        if (Player.Inputs.Default.Slide.ReadValue<float>() > 0f) {
            runner.SetState(player.States.SlideState);
        }

    }
    public override void PhysicsUpdate() {
        player.Move(Player.Inputs.Default.Movement.ReadValue<Vector2>(),Player.Inputs.Default.Sprint.ReadValue<float>() > 0,true);
    }
    public override void Exit() {
        Player.Inputs.Default.Jump.performed -= OnJump;

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
