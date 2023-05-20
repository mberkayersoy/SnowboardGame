using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    float rotateSpeed = 190f;
    public JumpState(PlayerController sm) : base(sm)
    {
    }


    public override void OnUpdate()
    {
        if (controller.Grounded)
        {
            controller.SetState(controller.movementState);
        }
        else if (controller.GetHeight() > 1)
        {
            if (controller.vInput == 0 && controller.hInput == 0)
            {
                controller.FixRotation();
            }
            else if (controller.hInput != 0)
            {
                controller.boardModel.Rotate(new Vector3(0, controller.hInput * 50f * Time.deltaTime, 0) * rotateSpeed * Time.deltaTime);
            }
            else if (controller.vInput != 0)
            {
                //controller.boardModel.Rotate(new Vector3(controller.vInput, 0, 0) * rotateSpeed * Time.deltaTime);
            }
        }

    }
}
