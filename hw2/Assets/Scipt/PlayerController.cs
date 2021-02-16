using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpSpeed = 9.0f;
    public float gravity = 20.0f;
    public Transform cameraParent;
    public float lookSensitivity = 3.0f;
    public float lookXMin = -35.0f;
    public float lookXMax = 60.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    public Camera BackCamera;
    public Camera TopCamera;

    public float deathHeight;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;

        BackCamera.enabled = true;
        TopCamera.enabled = false;
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.transform.TransformDirection(Vector3.right);
            float speedX = speed * Input.GetAxis("Vertical");
            float speedY = speed * Input.GetAxis("Horizontal");
            moveDirection = (forward * speedX) + (right * speedY);

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);

        rotation.y += Input.GetAxis("Mouse X") * lookSensitivity;
        rotation.x += -Input.GetAxis("Mouse Y") * lookSensitivity;
        rotation.x = Mathf.Clamp(rotation.x, lookXMin, lookXMax);
        cameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, rotation.y);

        if (transform.position.y < deathHeight)
        {
            transform.position = new Vector3(0, 0.8f, 0);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            BackCamera.enabled = !BackCamera.enabled;
            TopCamera.enabled = !TopCamera.enabled;
        }
    }
}
