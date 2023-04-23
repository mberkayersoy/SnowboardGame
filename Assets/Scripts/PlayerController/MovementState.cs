using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : PlayerState
{
    Vector3 movement;

    public MovementState(PlayerController sm) : base(sm)
    {
    }


    public override void OnUpdate()
    {
        if (!controller.Grounded)
        {
            controller.SetState(controller.jumpState);
            return;
        }
        else if (controller.isBreaking)
        {
            controller.SetState(controller.breakState);
            return;
        }

        if (controller.isJumping)
        {
            controller.sphere.AddForce(100 * controller.jumpStrength * Vector3.up);
        }

        controller.FixBoardYRotationOnGround();
        controller.OnSlope();

        movement = controller.boardModel.forward * controller.vInput;
        movement.y = 0f;

        controller.sphere.AddForce(movement.normalized * controller.acceleration, ForceMode.Force);

        Vector3 rotation = new Vector3(0.0f, controller.hInput / 2, 0.0f);
        controller.boardModel.Rotate(rotation);
    }
}
