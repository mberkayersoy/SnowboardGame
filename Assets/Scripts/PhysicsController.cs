using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{

  private Rigidbody rb;
  private BoardMove bm;
  public float speed = 600;
  private Vector3 slopeDirection;
  private float slopeAmount = 0;


  void Start()
  {
    rb = GetComponent<Rigidbody>();
    bm = GetComponent<BoardMove>();
  }

  void FixedUpdate()
  {

    Vector3 currentVelocity = rb.velocity;


    if (OnSlope() && bm.state != BoardingStates.Jump)
    {
      if (slopeAmount > 0)
      {
        Vector3 dr = rb.rotation * Vector3.forward;
        float angle = Vector3.Angle(dr, currentVelocity);
        if (angle > 10)
        {
          rb.AddForce(-currentVelocity.normalized * (currentVelocity.magnitude * (angle / 180)), ForceMode.Force);
        }
        rb.AddForce(dr * (slopeAmount * speed) * Time.fixedDeltaTime, ForceMode.Force);
      }
      else
      {
        Vector3 dr = rb.rotation * Vector3.forward;
        rb.AddForce(dr * (slopeAmount * speed) * Time.fixedDeltaTime, ForceMode.Force);
      }
    }


    currentVelocity = currentVelocity.normalized * Mathf.Clamp(currentVelocity.magnitude, BoardMove.minSpeed, BoardMove.maxSpeed);
    rb.velocity = currentVelocity;
  }

  private bool OnSlope()
  {
    Vector3 size = GetComponent<MeshCollider>().bounds.size;
    RaycastHit forwardHit;
    RaycastHit backHit;

    Vector3 boardHeadPosition = transform.position + (rb.rotation * (Vector3.forward * ((float)size.z * 0.5f)));
    Vector3 boardTailPosition = transform.position + (rb.rotation * (Vector3.back * ((float)size.z * 0.5f))); ;
    if (
        Physics.Raycast(boardHeadPosition, Vector3.down, out forwardHit, size.y * 0.5f + 0.3f) &&
        Physics.Raycast(boardTailPosition, Vector3.down, out backHit, size.y * 0.5f + 0.3f)
     )
    {
      Vector3 moveDirection = rb.rotation * Vector3.forward;

      slopeAmount = backHit.point.y - forwardHit.point.y;
      Vector3 slopeDirection = (forwardHit.point - backHit.point).normalized;

      Debug.DrawRay(boardHeadPosition, Vector3.down * 5, Color.blue);
      Debug.DrawRay(boardTailPosition, Vector3.down * 5, Color.red);
      Debug.DrawRay(boardTailPosition, slopeDirection * 6, Color.black);

      return slopeAmount > 0.3 ? true : false;
    }

    return false;
  }
}
