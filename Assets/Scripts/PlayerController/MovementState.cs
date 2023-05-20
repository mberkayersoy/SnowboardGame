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
            controller.sphere.AddForce(controller.jumpStrength * controller.boardModel.up.normalized, ForceMode.Impulse);
        }

        float angleToForward = Quaternion.Angle(controller.boardModel.rotation, Quaternion.identity);
        if (angleToForward <= 90f)
        {
            currentDirection = boardDirection;
        }
        else
        {
            currentDirection = -boardDirection;
        }

        controller.FixBoardYRotationOnGround();
        boardDirection = controller.boardModel.forward;
        boardDirection.y = 0f;
        controller.sphere.AddForce(controller.acceleration * -controller.boardModel.up, ForceMode.Acceleration);

        controller.sphere.AddForce(controller.acceleration * controller.vInput * boardDirection, ForceMode.Acceleration);

        Vector3 rotation = new Vector3(0.0f, controller.hInput, 0.0f);
        controller.boardModel.Rotate(rotation);
        Quaternion boardRotation = controller.boardModel.rotation;
        Vector3 rotationVector = boardRotation * currentDirection; // boardModel'in y eksenindeki yönü
        rotationVector.y = 0f; // sadece yönü etkileyecek vektör olarak saklanmasý için y eksenini sýfýrla
        controller.sphere.AddForce(rotationVector.normalized * controller.rotationSpeed, ForceMode.Acceleration);

        Debug.DrawRay(controller.boardNormal.position, this.currentDirection * 5, Color.red);
        Debug.DrawRay(controller.boardNormal.position, controller.sphere.velocity, Color.blue);
    }
}
