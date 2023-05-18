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

        controller.FixBoardYRotationOnGround();
        boardDirection = controller.boardModel.forward;
        boardDirection.y = 0f;

        controller.sphere.AddForce(controller.deceleration * controller.vInput * currentDirection, ForceMode.Force);

        if (controller.slopeAngle < 0)
        {
            //Debug.Log("egimden yukari");
            if (controller.GetLowerPoint())
            {
                currentDirection = boardDirection.normalized;
                controller.sphere.AddForce(currentDirection * controller.acceleration * Time.deltaTime , ForceMode.Acceleration);
                Debug.Log("Decelerating front");
            }
            else
            {
                currentDirection = -boardDirection.normalized;
                controller.sphere.AddForce(currentDirection * controller.acceleration * Time.deltaTime , ForceMode.Acceleration);
                Debug.Log("Decelerating tail");
            }
        }
        else if (Mathf.Abs(controller.slopeAngle) < 0.1f)
        {
            Vector3 lastDirection = controller.sphere.velocity.normalized;
            controller.sphere.AddForce(lastDirection * controller.acceleration * 0.5f , ForceMode.Acceleration);
            //Debug.Log("duz zemin");
        }
        else
        {
            //Debug.Log("egimden asagi");
            if (controller.GetLowerPoint())
            {
                currentDirection = boardDirection.normalized;
                controller.sphere.AddForce(currentDirection * controller.acceleration * 0.5f , ForceMode.Acceleration);
                Debug.Log("Accelerating front");
            }
            else
            {
                currentDirection = -boardDirection.normalized;
                controller.sphere.AddForce(currentDirection * controller.acceleration * 0.5f , ForceMode.Acceleration);
                Debug.Log("Accelerating tail");      
            }
        }

        Vector3 rotation = new Vector3(0.0f, controller.hInput, 0.0f);
        controller.boardModel.Rotate(rotation);
        Quaternion boardRotation = controller.boardModel.rotation;
        Vector3 rotationVector = boardRotation * currentDirection; // boardModel'in y eksenindeki yönü
        rotationVector.y = 0f; // sadece yönü etkileyecek vektör olarak saklanmasý için y eksenini sýfýrla
        controller.sphere.AddForce(rotationVector.normalized * controller.rotationSpeed, ForceMode.Acceleration);

        Debug.DrawRay(controller.boardNormal.position, this.currentDirection, Color.red);
        Debug.DrawRay(controller.boardNormal.position, controller.sphere.velocity, Color.blue);
    }
}
