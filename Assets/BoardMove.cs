using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum BoardingStates
{
  Skate,
  Break,
  Jump,
}

public class BoardMove : MonoBehaviour
{
  private Vector3 m_surfaceNormal = new Vector3();
  private Vector3 m_collisionPoint = new Vector3();
  private bool m_onSurface;
  private float minSpeed = 0f;
  private float maxSpeed = 30f;
  private float speed = 0f;
  private float RotationUnit = 1f;
  public float jumpStrength = 5;
  private Rigidbody rb;
  private Vector3 slopeDirection;

  private float yRotation = 0;
  private Vector3 velocity;

  public BoardingStates state = BoardingStates.Skate;

  VelocityCollider velocityCollider;
  public float groundOffset = 0.1f;
  private float characterHeight;
  float currentRotation;
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    velocityCollider = GetComponentInChildren<VelocityCollider>();
  }

  void Update()
  {
    Debug.Log("m_onSurface: " + m_onSurface);
    if (m_onSurface)
    {
      state = BoardingStates.Skate;
      // A/D Key: Change Board direction
      yRotation = Input.GetAxisRaw("Horizontal");
      LimitYRot();
      // Left Shift (Down): Change State as break, slow down speed and rotate board for break animation.

      Jump();
      //if (Input.GetKeyDown(KeyCode.LeftShift))
      //{
      //    state = BoardingStates.Break;
      //    //transform.DOLocalRotate(new Vector3(0, 90, 0), 1f, RotateMode.LocalAxisAdd);
      //    transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 90, 0), 1f);
      //}
      //// Left Shift (Up): Change State as Skate, rotate board as rotation before break.
      //else if (Input.GetKeyUp(KeyCode.LeftShift))
      //{
      //    transform.DOLocalRotate(new Vector3(0, -90, 0), 1f, RotateMode.LocalAxisAdd).OnComplete(() =>
      //    {
      //        state = BoardingStates.Skate;
      //    });
      //}

      // Space Key: Increase speed. Mostly for debugging.
      if (Input.GetKey(KeyCode.Space))
      {
        speed = Mathf.Clamp(speed + 0.1f, minSpeed, maxSpeed);
      }

      if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
      {
        Debug.Log("YEA");
        //rb.DORotate(Vector3.zero, 0.1f , RotateMode.LocalAxisAdd);
        //.DOLocalRotate(Vector3.zero, 10 * Time.deltaTime, RotateMode.LocalAxisAdd);
        //transform.DOLocalRotateQuaternion
        //transform.DORotateQuaternion(Vector3.zero, Time.deltaTime, RotateMode.LocalAxisAdd);
      }
    }


    if (!m_onSurface)
    {
      state = BoardingStates.Jump;
    }
  }

  private void LimitYRot()
  {
    Vector3 boardEulerAngles = transform.localRotation.eulerAngles;
    boardEulerAngles.y = (boardEulerAngles.y > 180) ? boardEulerAngles.y - 360 : boardEulerAngles.y;
    boardEulerAngles.y = Mathf.Clamp(boardEulerAngles.y, -90f, 90f);

    transform.localRotation = Quaternion.Euler(boardEulerAngles);
  }

  void FixedUpdate()
  {

    if (state == BoardingStates.Jump && GetHeight() > 2f)
    {
      float v = Input.GetAxisRaw("Vertical");
      float h = Input.GetAxisRaw("Horizontal");
      Debug.Log("FreesTyle" + "  h: " + h + "  v: " + v);
      rb.AddTorque(new Vector3(v, yRotation, h) * 5000);

      //Debug.Log("fixed");
      //float horizontalInput = Input.GetAxis("Horizontal");
      //float verticalInput = Input.GetAxis("Vertical");

      //Vector3 rotation = new Vector3(verticalInput, 0f, -horizontalInput) * 5000f;

      //rb.AddTorque(rotation);

      //if (!groundCheck.isGrounded) // Board rotation fix before land.
      //{
      //    Vector3 groundNormal = GetGroundNormal();

      //    if (groundNormal != Vector3.zero)
      //    {
      //        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
      //        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 3f * Time.deltaTime));
      //    }
      //}
    }
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
        if (m_onSurface)
        {
          // If there is no slope, decrease speed.
          speed = Mathf.Clamp(speed - 0.01f, minSpeed, maxSpeed);
        }
        // Todo: Should check if board touching the ground. If it is, we should not lower speed.
      }
    }


    if (BoardingStates.Jump != state)
    {
      velocity = GetVelocity();
      rb.velocity = velocity;
    }

  }

  void SetCurrentRotation(float rot)
  {
    yRotation = Mathf.Clamp(rot, -90, 90);
    rb.rotation = Quaternion.Euler(0, rot, 0);
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

  public float GetHeight()
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, Vector3.down, out hit))
    {
      characterHeight = hit.distance - groundOffset;   // charachter height from the land.
    }
    Debug.Log("Characher height: " + characterHeight);
    return characterHeight;
  }
  private bool OnSlope()
  {
    // Project a ray from pivot of object in the -y direction.
    // If it is hit to the surface, get slope angle and direction.
    float angle = Vector3.Angle(Vector3.up, m_collisionPoint);
    slopeDirection = Vector3.ProjectOnPlane(new Vector3(0, 0, 1), m_surfaceNormal).normalized;
    // If angle of slope bigger then 20. There is a slope that should effect speed of board. 
    return angle >= 20;
  }

  private void OnCollisionStay(Collision collision)
  {
    if (collision.gameObject.CompareTag("IceFloor"))
    {
      m_onSurface = true;
      m_surfaceNormal = collision.GetContact(0).normal;
      m_collisionPoint = collision.GetContact(0).point;
    }

  }
  private void OnCollisionExit(Collision collision)
  {
    if (collision.gameObject.CompareTag("IceFloor"))
    {
      m_onSurface = false;
    }
  }
  void Jump()
  {
    // Jump when the Jump button is pressed and we are on the ground.
    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      Debug.Log("Jumped");
      rb.AddForce(100 * jumpStrength * Vector3.up);
    }
  }
}
