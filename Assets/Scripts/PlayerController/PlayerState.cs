using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController controller;

    public PlayerState(PlayerController sm)
    {
        controller = sm;
    }

    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}
