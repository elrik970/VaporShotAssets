using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "FallingState", menuName = "States/Player/FallingState")]
public class FallingState : PlayerState<Player>
{
    public float fallingGravityScale;

    public override void Init(Player parent) {
        base.Init(parent);
        player.SetGravity(fallingGravityScale);
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
        
        if (player.OnGround()) {
            runner.SetState(player.States.IdleState);
        }
        if (rb.velocity.y > 0f) {
            runner.SetState(player.States.RisingState);
        }
        player.Move(Player.Inputs.Default.Movement.ReadValue<Vector2>());
    }
    public override void Exit() {
        player.SetGravity(player.Forces.defaultGravityScale);
        Player.Inputs.Default.Dash.performed -= OnDash;

    }
    void OnDash(InputAction.CallbackContext context) {
        player.Dash();
    }
}
