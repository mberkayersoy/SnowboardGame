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
        Vector3 currentUp = controller.boardNormal.up.normalized;
        controller.sphere.AddForce(-currentUp * 16, ForceMode.Force); // yerden ayrýlmamak için þimdilik bir yöntem

        //movement = controller.boardModel.forward * controller.vInput;
        //movement.y = 0f;


        movement = controller.boardModel.forward; //* controller.vInput;///+  controller.OnSlope() * controller.acceleration;
        movement.y = 0f;
        Vector3 movementDirection = movement.normalized;
        Vector3 boardDirection = controller.boardModel.forward;
        Vector3 velocityDirection = controller.sphere.velocity.normalized;
        Vector3 surfaceSlope = controller.OnSlope().normalized;
        Vector3 perpDirection = Vector3.Cross(movementDirection, surfaceSlope);
        Vector3 movementOnSlope = Vector3.Cross(surfaceSlope, perpDirection).normalized * movement.magnitude;
        // tahtanýn normalinin ve hareket yönünün çarpýmýnýn iþaretini kontrol ediyoruz
        float dotProduct = Vector3.Dot(movementOnSlope, velocityDirection);

        bool isBoardFacingForward = dotProduct > 0;
        if (isBoardFacingForward)
        {
            controller.sphere.AddForce(boardDirection * controller.acceleration, ForceMode.Force);
        }
        else
        {
            controller.sphere.AddForce(-boardDirection * controller.acceleration, ForceMode.Force);
        }
        // controller.sphere.AddForce(movement * 5, ForceMode.Force);
        //controller.sphere.AddForce(controller.boardModel.forward * controller.vInput * controller.acceleration / 3, ForceMode.Force);

        Vector3 rotation = new Vector3(0.0f, controller.hInput / 2, 0.0f);
        controller.boardModel.Rotate(rotation);
        Debug.DrawRay(controller.boardNormal.position, movement, Color.red);
        Debug.DrawRay(controller.boardNormal.position, controller.sphere.velocity, Color.blue);
        Debug.DrawRay(controller.boardNormal.position, controller.OnSlope() * 5, Color.green);
    }

    /*
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

        RaycastHit hit;
        Physics.Raycast(controller.boardNormal.position, -controller.boardNormal.position, out hit);

        Vector3 currentUp = controller.boardNormal.up.normalized;

        controller.sphere.AddForce(-currentUp.normalized * 16, ForceMode.Force); // yerden ayrýlmamak için þimdilik bir yöntem

        Vector3 boardDirection = controller.boardModel.forward;
        Vector3 movementDirection = movement.normalized;

        // tahtanýn normalinin ve hareket yönünün çarpýmýnýn iþaretini kontrol ediyoruz
        float dotProduct = Vector3.Dot(controller.boardNormal.up, movementDirection);
        bool isBoardFacingForward = dotProduct > 0;

        // tahtanýn ön tarafý önde ise harekete o yönde devam ediyoruz
        if (isBoardFacingForward)
        {
            controller.sphere.AddForce(boardDirection * controller.acceleration, ForceMode.Force);
        }
        else // arka tarafý önde ise hareket yönünün tersi yönde hareket ediyoruz
        {
            controller.sphere.AddForce(-boardDirection * controller.acceleration, ForceMode.Force);
        }

        //controller.sphere.AddForce(movement.normalized * controller.acceleration, ForceMode.Force);

        Vector3 rotation = new Vector3(0.0f, controller.hInput / 2, 0.0f);
        controller.boardModel.Rotate(rotation);
    }
}
      */
}
