using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "WallRunningLeftState", menuName = "States/Player/WallRunningLeftState")]
public class WallRunningLeftState : PlayerState<Player>
{
    public float wallRunGravityScale;
    public float horizontalJumpPower;
    public Vector3 DirectionToWall;
    public float RotateSpeed = 30f;
    public override void Init(Player parent) {
        base.Init(parent);

        DirectionToWall = -player.Char.Body.right;

        rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);

        player.SetGravity(wallRunGravityScale);

        Player.Inputs.Default.Jump.performed += OnJump;

        player.Forces.curCoyoteTime = player.Forces.coyoteTime;
        


    }
    public override void ConstantUpdate() {
        if (player.HeadControls.zAngle > -15f) {
            player.HeadControls.zAngle += Time.deltaTime * RotateSpeed;
        }
    }
    public override void CaptureInputs() {

    }
    public override void ChangeState() {
        if (!player.OnWall(DirectionToWall)) {
            runner.SetState(player.States.FallingState);
            player.Forces.curWallCoolDown = 0f;
        }
    }
    public override void PhysicsUpdate() {
        player.Move(Player.Inputs.Default.Movement.ReadValue<Vector2>(),Player.Inputs.Default.Sprint.ReadValue<float>() > 0);
    }
    public override void Exit() {
        Player.Inputs.Default.Jump.performed -= OnJump;
        player.SetGravity(player.Forces.defaultGravityScale);

        player.Forces.canDash = true;


        player.HeadControls.zAngle = 0f;

        player.Forces.curCoyoteTime = 0f;


        
    }

    void OnJump(InputAction.CallbackContext context) {
        player.Jump();
        rb.AddForce(player.Char.Body.right*horizontalJumpPower,ForceMode.Impulse);
        runner.SetState(player.States.JumpState);

        player.Forces.curWallCoolDown = 0;
    }
}
