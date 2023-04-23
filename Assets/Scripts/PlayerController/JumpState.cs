using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{

    public JumpState(PlayerController sm) : base(sm)
    {
    }


    public override void OnUpdate()
    {
        if (controller.Grounded)
        {
            controller.SetState(controller.movementState);
        }
        else if (controller.GetHeight() > 2)
        {
            if (controller.vInput == 0 && controller.hInput == 0)
            {
                controller.FixRotation();
            }
            else
            {
                controller.boardModel.Rotate(new Vector3(controller.vInput, 0f, controller.hInput) * 60 * Time.deltaTime);
            }

        }

    }
}
