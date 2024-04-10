using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "RisingState", menuName = "States/Player/RisingState")]
public class RisingState : PlayerState<Player>
{
    public float risingGravityScale;

    public override void Init(Player parent) {
        base.Init(parent);
        player.SetGravity(risingGravityScale);
        Player.Inputs.Default.Dash.performed += OnDash;

        

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
        player.Move(Player.Inputs.Default.Movement.ReadValue<Vector2>());
        if (rb.velocity.y <= 0f) {
            runner.SetState(player.States.FallingState);
        }
        if (player.OnGround()) {
            runner.SetState(player.States.IdleState);
        }
    }
    public override void Exit() {
        player.SetGravity(player.Forces.defaultGravityScale);
        Player.Inputs.Default.Dash.performed -= OnDash;
    }
    void OnDash(InputAction.CallbackContext context) {
        player.Dash();
    }
}
