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
            controller.tailVFX.SetActive(false);
            controller.SetState(controller.jumpState);
            return;
        }
        else if (controller.isBreaking)
        {
            controller.tailVFX.SetActive(false);
            controller.SetState(controller.breakState);
            return;
        }

        if (controller.isJumping)
        {
            controller.sphere.AddForce(controller.jumpStrength * controller.boardModel.up.normalized, ForceMode.Impulse);

            return;
        }
        controller.FixBoardYRotationOnGround();
        boardDirection = controller.boardModel.forward;

        float rotationY = controller.hInput * 50f * Time.deltaTime;
        Vector3 rotation = new Vector3(0.0f, rotationY, 0.0f);
        controller.boardModel.Rotate(rotation);
    }

    public override void OnFixedUpdate()
    {

        currentDirection = boardDirection.normalized;
        controller.rightVFX.SetActive(false);
        controller.sphere.AddForce(200 * Time.fixedDeltaTime * -controller.boardModel.up, ForceMode.Acceleration);

        Vector3 force = controller.acceleration * controller.vInput * Time.fixedDeltaTime * currentDirection;
        controller.sphere.AddForce(force, ForceMode.Acceleration);

        controller.FixBoardYRotationOnGround();

        // Velocity update for easy rotation
        float y = controller.sphere.velocity.y;
        Vector3 velocityDirection = controller.sphere.velocity.normalized;
        Vector3 newVelocity = Vector3.Lerp(currentDirection, velocityDirection, 50f * Time.deltaTime) * controller.sphere.velocity.magnitude;
        newVelocity.y = y;
        controller.sphere.velocity = newVelocity;

        Debug.DrawRay(controller.boardNormal.position, currentDirection, Color.red);
        Debug.DrawRay(controller.boardNormal.position, controller.sphere.velocity, Color.blue);
    }

}
