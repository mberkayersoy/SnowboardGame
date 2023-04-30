using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : PlayerState
{
    Vector3 boardDirection;
    Vector3 currentDirection;
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
            controller.sphere.AddForce(controller.jumpStrength * Vector3.up, ForceMode.Impulse);
        }

        controller.FixBoardYRotationOnGround();
        boardDirection = controller.boardModel.forward;
        boardDirection.y = 0f;

        if (controller.slopeAngle < 0)
        {
            Debug.Log("egimden yukari");
            if (controller.GetLowerPoint())
            {
                controller.sphere.AddForce(boardDirection * controller.deceleration, ForceMode.Acceleration);
                Debug.Log("Decelerating front");
            }
            else
            {
                controller.sphere.AddForce(-boardDirection * controller.deceleration, ForceMode.Acceleration);
                Debug.Log("Decelerating tail");
            }
        }
        else if (Mathf.Abs(controller.slopeAngle) < 0.1f)
        {
            Vector3 lastDirection = controller.sphere.velocity.normalized;
            controller.sphere.AddForce(lastDirection * controller.acceleration, ForceMode.Acceleration);
            Debug.Log("duz zemin");
        }
        else
        {
            Debug.Log("egimden asagi");
            if (controller.GetLowerPoint())
            {
                controller.sphere.AddForce(boardDirection * controller.acceleration, ForceMode.Acceleration);
                Debug.Log("Accelerating front");
            }
            else
            {
                controller.sphere.AddForce(-boardDirection * controller.acceleration, ForceMode.Acceleration);
                Debug.Log("Accelerating tail");
            }
        }
        Vector3 rotation = new Vector3(0.0f, controller.hInput / 3, 0.0f);
        controller.boardModel.Rotate(rotation);

        Debug.DrawRay(controller.boardNormal.position, this.boardDirection, Color.red);
        Debug.DrawRay(controller.boardNormal.position, controller.sphere.velocity, Color.blue);
    }
}
