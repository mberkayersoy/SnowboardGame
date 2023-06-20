using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody sphere;
    public Animator animator;
    public GameObject tailVFX;
    public GameObject leftVFX;
    public GameObject rightVFX;

    [Header("Transforms")]
    public Transform boardNormal;
    public Transform boardModel;
    public Transform playerModel;
    public Transform boardFrontHit1;
    public Transform boardTailHit2;
    public Transform boardRight;
    public Transform boardLeft;

    [Header("Board Physics Variables")]
    public float jumpStrength = 10f;
    public float acceleration = 0.5f;
    public float deceleration = 0.1f;
    public float maxSpeed = 55f;
    public float rotationSpeed;
    public Vector3 boardOffSet;
    public Vector3 playerOffSet;



    [Header("Grounded")]

    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = 0.1f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.8f;
    public LayerMask GroundLayers;
    public float slopeAngle;
    public Vector3 groundNormal;

    [Header("Inputs")]
    public float hInput;
    public float vInput;
    public bool isJumping = false;
    public bool isBreaking = false;


    [Header("State")]
    // States
    public PlayerState CurrentState;
    public PlayerState movementState;
    public PlayerState jumpState;
    public PlayerState breakState;

    public void SetState(PlayerState state)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }

        CurrentState = state;
        CurrentState.OnStart();
    }

    void Start()
    {
        movementState = new MovementState(this);
        jumpState = new JumpState(this);
        breakState = new BreakState(this);
        animator = playerModel.gameObject.GetComponent<Animator>();
        tailVFX = Instantiate(tailVFX, boardFrontHit1.position, Quaternion.Euler(-90, 0 , 0), boardFrontHit1);
        SetState(movementState);
    }

    public void ChangeVFX(float rotationWay)
    {
        if (rotationWay == 1)
        {
            tailVFX = Instantiate(leftVFX, boardRight.position, Quaternion.identity, boardRight);
        }
        else
        {
            tailVFX = Instantiate(leftVFX, boardLeft.position, Quaternion.identity, boardLeft);
        }

    }

    private void LateUpdate()
    {
        //boardModel.position = sphere.position - boardOffSet;
        boardModel.position = Vector3.Lerp(boardModel.position, sphere.position - boardOffSet, Time.deltaTime * 100);
        playerModel.position = Vector3.Lerp(playerModel.position, boardModel.position - playerOffSet, Time.deltaTime * 100);

    }

    void Update()
    {
        GetInputs();
        if (CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
    }

    public IEnumerator PlayAnim(bool isWon)
    {
        if (!isWon)
        {
            tailVFX.SetActive(false);
            animator.SetBool("FallingBack", true);
            sphere.isKinematic = true;
        }
        else
        {
            sphere.drag = 3;
            yield return new WaitForSeconds(1.5f);
            animator.SetBool("Victory", true);
            tailVFX.SetActive(false);
        }

    }
    private void FixedUpdate()
    {
        //slopeAngle = GetSlopeAngle();
        GroundedCheck();

        if (CurrentState != null)
        {
            CurrentState.OnFixedUpdate();
        }

        // Clamp Velocity
        sphere.velocity = Vector3.ClampMagnitude(sphere.velocity, maxSpeed);
    }

    void GetInputs()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Mathf.Max(Input.GetAxis("Vertical"), 0);
        isJumping = Input.GetKeyDown(KeyCode.Space);
        isBreaking = Input.GetKey(KeyCode.LeftShift);
    }
    void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(boardModel.position.x, boardModel.position.y - GroundedOffset,
            boardModel.position.z + 0.5f);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }

    public void FixBoardYRotationOnGround()
    {
        float rotationSpeed = 100f;
        RaycastHit hit1;
        RaycastHit hit2;
        Quaternion targetRotation;
        if (Physics.Raycast(boardTailHit2.position, Vector3.down, out hit1))
        {
            Vector3 groundNormal = hit1.normal;
            Quaternion groundRotation = Quaternion.FromToRotation(boardModel.up, groundNormal);
            targetRotation = Quaternion.RotateTowards(boardModel.rotation, groundRotation * boardModel.rotation, rotationSpeed * Time.deltaTime);
            boardModel.rotation = targetRotation;
        }
        if (Physics.Raycast(boardFrontHit1.position, Vector3.down, out hit2))
        {
            Vector3 groundNormal = hit2.normal;
            Quaternion groundRotation = Quaternion.FromToRotation(boardModel.up, groundNormal);
            targetRotation = Quaternion.RotateTowards(boardModel.rotation, groundRotation * boardModel.rotation, rotationSpeed * Time.deltaTime);
            boardModel.rotation = targetRotation;
        }
    }
    //public bool GetLowerPoint()
    //{
    //    RaycastHit frontHit1, frontHit2, backHit1, backHit2;

    //    bool front1 = Physics.Raycast(boardFrontHit1.position, Vector3.down, out frontHit1, 20f);
    //    bool front2 = Physics.Raycast(boardFrontHit2.position, Vector3.down, out frontHit2, 20f);
    //    bool back1 = Physics.Raycast(boardTailHit1.position, Vector3.down, out backHit1, 20f);
    //    bool back2 = Physics.Raycast(boardTailHit2.position, Vector3.down, out backHit2, 20f);


    //    float frontHeight = float.MinValue, backHeight = float.MinValue;
    //    if (front1 || front2)
    //    {
    //        frontHeight = Mathf.Max(frontHit1.point.y, frontHit2.point.y);
    //    }

    //    if (back1 || back2)
    //    {
    //        backHeight = Mathf.Max(backHit1.point.y, backHit2.point.y);
    //    }

    //    if (frontHeight > backHeight)
    //    {
    //        return false;
    //    }
    //    else if (backHeight > frontHeight)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
    public float GetHeight()
    {
        RaycastHit hit;
        Physics.Raycast(boardNormal.position, Physics.gravity, out hit);
        //Debug.DrawRay(boardNormal.position, Physics.gravity, Color.red);
        return hit.distance;
    }

    public void FixRotation()
    {
        RaycastHit hit;
        Physics.Raycast(boardNormal.position, -Vector3.up, out hit);
        var localRot = Quaternion.FromToRotation(boardModel.up, hit.normal) * boardModel.rotation;
        var euler = localRot.eulerAngles;
        euler.y = 0;
        localRot.eulerAngles = euler;
        boardModel.localRotation = Quaternion.LerpUnclamped(boardModel.localRotation, localRot, 3f * Time.deltaTime);
    }

    //    public float GetSlopeAngle()
    //    {
    //        // Get the normal of the ground below the object
    //        RaycastHit hit;
    //        if (Physics.Raycast(boardNormal.position, -boardNormal.up, out hit))
    //        {
    //            groundNormal = hit.normal;
    //            float tmpAngle = Vector3.Angle(groundNormal, Vector3.up) * Mathf.Sign(Vector3.Dot(groundNormal, Vector3.Cross(transform.right, Vector3.up)));
    //            return tmpAngle;
    //            if (tmpAngle <= 0)
    //            {
    //                if (GetLowerPoint())
    //                {
    //                    return tmpAngle;
    //                }
    //                else
    //                {
    //                    return -tmpAngle;
    //                }
    //            }
    //            else
    //            {
    //                if (GetLowerPoint())
    //                {
    //                    return tmpAngle;
    //                }
    //                else
    //                {
    //                    return -tmpAngle;
    //                }
    //            }

    //        }
    //        return 0;
    //    }

}
