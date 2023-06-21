using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    float rotateSpeed = 320;
    private float totalRotation;
    public JumpState(PlayerController sm) : base(sm)
    {
    }


    public override void OnUpdate()
    {
        if (controller.Grounded)
        {
            GetTotalRotation();
            controller.animator.SetBool("onJumpState", false);
            controller.SetState(controller.movementState);
            totalRotation = 0;
        }
        else if (controller.GetHeight() > 1)
        {
            if (controller.vInput == 0 && controller.hInput == 0)
            {
                controller.FixRotation();
            }
            else if (controller.hInput != 0)
            {
                float rotationAmount = controller.hInput * rotateSpeed * Time.deltaTime;
                controller.boardModel.Rotate(new Vector3(0, controller.hInput, 0) * rotateSpeed * Time.deltaTime);
                totalRotation += Mathf.Abs(rotationAmount); // Toplam rotasyon miktar�n� g�ncelle
            }
            //else if (controller.vInput != 0)
            //{
            //    controller.boardModel.Rotate(new Vector3(controller.vInput, 0, 0) * rotateSpeed * Time.deltaTime);
            //}
            //    else if (controller.hInput != 0 || controller.vInput != 0)
            //    {
            //        controller.boardModel.Rotate(new Vector3(controller.vInput, controller.hInput, 0) * rotateSpeed * Time.deltaTime);
            //    }

            controller.animator.SetBool("onJumpState", true);
        }

    }

    // TotalRotation de�i�kenini d��ar� aktarmak i�in bir y�ntem
    public float GetTotalRotation()
    {
        Debug.Log("total: " + totalRotation);
        return totalRotation;
    }
}

