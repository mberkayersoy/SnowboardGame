using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakState : PlayerState
{
    Transform beforeBreak;
    float RotationAmount = 0;
    float RotationWay = 1;

    public BreakState(PlayerController sm) : base(sm)
    {
    }

    public override void OnStart()
    {
        beforeBreak = controller.gameObject.transform;
        RotationAmount = 0;
    }

    // Update is called once per frame
    public override void OnUpdate()
    {

        if (!controller.isBreaking && RotationAmount <= 0)
        {
            // Change to MovementState
            controller.SetState(controller.movementState);
        }
        else if (!controller.isBreaking && RotationAmount > 0)
        {
            // Undo Rotate
            Vector3 rotation = new Vector3(0.0f, -90 * Time.deltaTime * RotationWay, 0.0f);
            controller.boardModel.Rotate(rotation);
            RotationAmount = RotationAmount - (90 * Time.deltaTime);
            controller.FixBoardYRotationOnGround();
        }
        else if (controller.isBreaking && RotationAmount <= 90)
        {
            if (controller.isBreaking && RotationAmount <= 0)
            {
                // Start Break
                if (controller.hInput < 0) {
                    RotationWay = -1;
                }
                else
                {
                    RotationWay = 1;
                }

            }
            // Rotate For Break
            Vector3 rotation = new Vector3(0.0f, 90 * Time.deltaTime * RotationWay, 0.0f);
            controller.boardModel.Rotate(rotation);
            RotationAmount = RotationAmount + (90 * Time.deltaTime);
            controller.FixBoardYRotationOnGround();
        }
        else if (!controller.isBreaking && RotationAmount > 0)
        {
            //Undo Rotate
            Vector3 rotation = new Vector3(0.0f, -90 * Time.deltaTime, 0.0f);
            controller.boardModel.Rotate(rotation);
            RotationAmount = RotationAmount - (90 * Time.deltaTime);
            controller.FixBoardYRotationOnGround();
        }

        if (controller.isBreaking)
        {
            // Stop Move
            float magnitude = controller.sphere.velocity.magnitude;
            controller.sphere.velocity = controller.sphere.velocity.normalized * Mathf.Max(magnitude - magnitude * Time.deltaTime, 0);
        }
    }
}
