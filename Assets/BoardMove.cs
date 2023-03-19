using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum BoardingStates
{
  Skate,
  Break,
}

public class BoardMove : MonoBehaviour
{
  private float minSpeed = 0f;
  private float maxSpeed = 30f;
  private float speed = 0f;
  private float RotationUnit = 1f;
  private Rigidbody rb;
  private Vector3 slopeDirection;

  private float yRotation = 0;
  private Vector3 velocity;

  public BoardingStates state = BoardingStates.Skate;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }


  void Update()
  {
    // A/D Key: Board rotation
    yRotation = Input.GetAxisRaw("Horizontal");


    // Space Key: Increase speed. Mostly for debugging.
    if (Input.GetKey(KeyCode.Space))
    {
      speed = Mathf.Clamp(speed + 0.1f, minSpeed, maxSpeed);
    }

    // Left Shift (Down): Change State as break, slow down speed and rotate board for break animation.
    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      state = BoardingStates.Break;
      transform.DOLocalRotate(new Vector3(0, 90, 0), 1f, RotateMode.LocalAxisAdd);
    }
    // Left Shift (Up): Change State as Skate, rotate board as rotation before break.
    else if (Input.GetKeyUp(KeyCode.LeftShift))
    {
      transform.DOLocalRotate(new Vector3(0, -90, 0), 1f, RotateMode.LocalAxisAdd).OnComplete(() =>
      {
        state = BoardingStates.Skate;
      });
    }
  }


  void FixedUpdate()
  {

    if (state == BoardingStates.Break)
    {
      // On break state. Decrase speed and stop the board.
      speed = Mathf.Clamp(speed - 0.5f, minSpeed, maxSpeed);
    }
    else
    {
      // Angular Velocity for left-right rotation.
      rb.angularVelocity = (new Vector3(0, yRotation, 0)) * RotationUnit;


      // Skating Physics
      if (OnSlope())
      {
        // Angle Between direction of movement and slope.
        float angle = Vector3.Angle(rb.rotation * (new Vector3(0, 0, 1)), slopeDirection);

        // If direction of movement and slope are similar increase speed, otherwise decrease.
        if (angle < 30 || angle > 330) speed = Mathf.Clamp(speed + 0.1f, minSpeed, maxSpeed);
        else if (angle < 45 || angle > 305) speed = Mathf.Clamp(speed + 0.05f, minSpeed, maxSpeed);
        else if (angle > 45 && angle < 90) speed = Mathf.Clamp(speed - 0.05f, minSpeed, maxSpeed);
        else if (angle > 90 && angle < 270) speed = Mathf.Clamp(speed - 0.2f, minSpeed, maxSpeed);
      }
      else
      {
        // If there is no slope, decrease speed.
        speed = Mathf.Clamp(speed - 0.01f, minSpeed, maxSpeed);

        // Todo: Should check if board touching the ground. If it is, we should not lower speed.
      }


    }

    velocity = GetVelocity();
    rb.velocity = velocity;
  }

  private Vector3 GetVelocity()
  {
    // Calculates velocity. Not modifies y axis, beacuse of default gravity.
    Vector3 currentV = rb.velocity;
    Vector3 newV = rb.rotation * (new Vector3(0, 0, 1) * speed);

    newV.y = 0;
    newV.y = currentV.y;

    return newV;
  }

  private bool OnSlope()
  {

    Vector3 size = GetComponent<BoxCollider>().bounds.size;
    RaycastHit slopeHit;

    if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, size.y * 0.5f + 0.3f))
    {
      // Project a ray from pivot of object in the -y direction.
      // If it is hit to the surface, get slope angle and  direction.
      float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
      slopeDirection = Vector3.ProjectOnPlane(new Vector3(0, 0, 1), slopeHit.normal).normalized;

      // If angle of slope bigger then 20. There is a slope that should effect speed of board. 
      return angle < 20 ? false : true;
    }

    return false;
  }

}
