using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarMovemant : MonoBehaviour
{
  public float force = 5.0f;
  private float RotationUnit = 60f;
  private Rigidbody rb;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void FixedUpdate()
  {

    float yRotation = Input.GetAxisRaw("Horizontal");

    // rb.angularVelocity = (new Vector3(0, yRotation, 0)) * RotationUnit;
    rb.AddRelativeTorque((new Vector3(0, yRotation, 0)) * RotationUnit);

    if (Input.GetKey(KeyCode.UpArrow))
    {
      rb.AddForce(rb.rotation * (new Vector3(1, 0, 0)) * force * Time.deltaTime);
    }



    // rb.AddForce(rb.rotation * (new Vector3(0, 0, 1)) * force * Time.deltaTime);
    // rb.AddForce(rb.rotation * (new Vector3(0, 0, -1)) * force * Time.deltaTime);
    // rb.MoveRotation(Quaternion.Euler(Mathf.MoveTowardsAngle(FixAngle(rb.rotation.eulerAngles.x), 0.0f, 0.1f * Time.deltaTime), rb.rotation.eulerAngles.y, rb.rotation.eulerAngles.z));
  }
}
