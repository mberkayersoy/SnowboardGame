using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour
{
    public Rigidbody sphere;
    public Transform boardNormal;
    public Transform boardModel;
    public Transform boardFront;
    public Transform boardTail;

    [Header("Parameters")]
    public float jumpStrength = 10f;
    public float acceleration = 30f;
    public float gravity = 10f;
    public Vector3 boardOffSet;
    Vector3 movement;
    float hInput;
    float vInput;

    [Header("Grounded")]

    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = 1;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;
    public float slopeLimit;
    public float slopeFriction;
    float slopeAngle;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = Vector3.zero;
        boardModel.position = Vector3.SmoothDamp(boardModel.position, sphere.position - boardOffSet, ref currentVelocity, 0.125f, 2f);

        //boardModel.position = sphere.position - boardOffSet;
        GroundedCheck();
        if (Grounded)
        {
            FixBoardYRotationOnGround();
            OnSlope();
            //    if (slopeAngle > slopeLimit && slopeAngle < 90f)
            //    {
            //        sphere.velocity = new Vector3(sphere.velocity.x * slopeFriction, sphere.velocity.y, sphere.velocity.z * slopeFriction);
            //        Vector3 targetDirection = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
            //        transform.rotation = Quaternion.LookRotation(targetDirection, hit.normal);
            //    }

            hInput = Input.GetAxis("Horizontal") / 2;
            vInput = Input.GetAxis("Vertical");

            movement = boardModel.forward * vInput;
            movement.y = 0f;

            sphere.AddForce(movement.normalized * acceleration, ForceMode.Force);

            Vector3 rotation = new Vector3(0.0f, hInput, 0.0f);
            boardModel.Rotate(rotation);

        }

        sphere.velocity = Vector3.ClampMagnitude(sphere.velocity, 35f);


        if (!Grounded && GetHeight() > 2)
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                Debug.Log("F�X ROTAT�ON");
                FixRotation();
            }

        }

    }

    private void FixedUpdate()
    {
        Jump();
        FreeStyle();
    }

    void FreeStyle()
    {
        if (!Grounded && GetHeight() > 2)
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");
            //sphere.AddTorque(new Vector3(v, 0f, h) * 10000);
            boardModel.Rotate(new Vector3(v, 0f, h) * 5);
        }
    }

    void Jump()
    {
        if (!Grounded) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jumped");
            sphere.AddForce(100 * jumpStrength * Vector3.up);
        }
    }

    public float GetHeight()
    {
        RaycastHit hit;
        Physics.Raycast(boardNormal.position, Physics.gravity, out hit);
        //Debug.DrawRay(boardNormal.position, Physics.gravity, Color.red);
        return hit.distance;

    }

    void FixBoardYRotationOnGround()
    {
        float rotationSpeed = 10f;
        RaycastHit hit1;
        RaycastHit hit2;
        Quaternion targetRotation;
        if (Physics.Raycast(boardFront.position, Vector3.down, out hit1))
        {
            Vector3 groundNormal = hit1.normal;
            Quaternion groundRotation = Quaternion.FromToRotation(boardModel.up, groundNormal);
            targetRotation = Quaternion.RotateTowards(boardModel.rotation, groundRotation * boardModel.rotation, rotationSpeed * Time.deltaTime);
            boardModel.rotation = targetRotation;
        }
        if (Physics.Raycast(boardTail.position, Vector3.down, out hit2))
        {
            Vector3 groundNormal = hit2.normal;
            Quaternion groundRotation = Quaternion.FromToRotation(boardModel.up, groundNormal);
            targetRotation = Quaternion.RotateTowards(boardModel.rotation, groundRotation * boardModel.rotation, rotationSpeed * Time.deltaTime);
            boardModel.rotation = targetRotation;
        }
    }

    private void FixRotation()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit);
        var localRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        var euler = localRot.eulerAngles;
        euler.y = 0;
        localRot.eulerAngles = euler;
        boardModel.localRotation = Quaternion.LerpUnclamped(boardModel.localRotation, localRot, 2 * Time.deltaTime);
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(boardModel.position.x, boardModel.position.y - GroundedOffset,
            boardModel.position.z + 0.5f);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }

    float OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(boardNormal.position, Vector3.down, out hit))
        {
            slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            Vector3 slopeDirection = Vector3.Cross(hit.normal, Vector3.down);

            Debug.Log(slopeAngle + " slope direction: " + slopeDirection);
            return slopeAngle;
        }
        else
        {
            return 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(boardModel.position.x, boardModel.position.y - GroundedOffset, boardModel.position.z + 0.5f),
            GroundedRadius);
    }
}
