using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerC : MonoBehaviour
{
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public Transform boardModel;
    private CharacterController controller;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (true)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }

        moveDirection.y += gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        Vector3 rotation = new Vector3(0.0f, Input.GetAxis("Horizontal") / 3, 0.0f);
        boardModel.Rotate(rotation);
        boardModel.position = transform.position;
    }
}
