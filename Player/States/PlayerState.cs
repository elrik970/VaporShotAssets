using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState<Player> : ScriptableObject where Player : MonoBehaviour
{
    public StateManager runner;
    public Player player;
    public Rigidbody rb;
    
    public virtual void Init(Player parent) {
        player = parent;
        runner = parent.GetComponent<StateManager>();
        rb = parent.GetComponent<Rigidbody>();
    }

    public abstract void ConstantUpdate();
    public abstract void CaptureInputs();
    public abstract void ChangeState();
    public abstract void PhysicsUpdate();
    public abstract void Exit();
    
}
