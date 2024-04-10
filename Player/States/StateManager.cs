using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public PlayerState<Player> curState;
    public Player player;
    void Start()
    {
        player = GetComponent<Player>();
        curState.Init(player);
    }

    // Update is called once per frame
    void Update()
    {
        
        curState.CaptureInputs();
        curState.ConstantUpdate();
    }
    void FixedUpdate() 
    {
        curState.ChangeState();
        curState.PhysicsUpdate();
    }
    public void SetState(PlayerState<Player> stateToChangeTo) {
        if (curState != null) {
            curState.Exit();
        }
        curState = stateToChangeTo;
        stateToChangeTo.Init(player);
    }
}
