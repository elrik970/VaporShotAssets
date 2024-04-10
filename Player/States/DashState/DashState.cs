using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


[CreateAssetMenu(fileName = "DashState", menuName = "States/Player/DashState")]
public class DashState : PlayerState<Player>
{
    public float DashSpeed;
    private float tempDashSpeed;
    private Vector3 moveDir;
    public float DashTime;
    private float curDashTime;
    public float velocityDivider;
    public VolumeProfile DashVolume;
    public Color DashColor;
    public float CoyoteTime;


    public override void Init(Player parent) {
        base.Init(parent);
        player.SetGravity(0f);
        Player.Inputs.Default.Jump.performed += OnJump;
        moveDir = player.HeadControls.Head.forward;
        curDashTime = 0f;
        player.Forces.canDash = false;
        // player.ParticleFX.GlobalVolume.profile = DashVolume;
        var main = player.ParticleFX.speedEffect.main;
        main.startColor = DashColor;
        player.Forces.curCoyoteTime = player.Forces.coyoteTime;
        player.ParticleFX.Cursor.color = DashColor;

        player.ParticleFX.speedEffect.Play();

        Vector3 nonYvelocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        if (nonYvelocity.magnitude > DashSpeed) {
            tempDashSpeed = nonYvelocity.magnitude;
        }
        else {
            tempDashSpeed = DashSpeed;
        }


    }
    public override void ConstantUpdate() {
        curDashTime+=Time.deltaTime;
        if (curDashTime > DashTime) {
            runner.SetState(player.States.FallingState);
        }
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
        rb.velocity = tempDashSpeed * moveDir;
    }
    public override void Exit() {
        player.SetGravity(player.Forces.defaultGravityScale);
        rb.velocity = rb.velocity / velocityDivider;
        player.Forces.canDash = false;
        // player.Forces.curCoyoteTime = player.Forces.coyoteTime-CoyoteTime;
        Player.Inputs.Default.Jump.performed -= OnJump;
        // player.ParticleFX.GlobalVolume.profile = player.ParticleFX.DefaultVolume;
        var main = player.ParticleFX.speedEffect.main;
        main.startColor = player.ParticleFX.defaultSpeedEffectColor;
        player.ParticleFX.Cursor.color = player.ParticleFX.defaultCursorColor;
        player.Forces.curDashTime = 0f;
    }
    void OnJump(InputAction.CallbackContext context) {
        runner.SetState(player.States.JumpState);
        player.Jump();
        player.DoubleJumpFx();
    }
}
