using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    [Header("Move")]
    [SerializeField] float speed;
    [SerializeField] VariableJoystick variableJoystick;

    [Header("Gravity")]
    [SerializeField] float gravity;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    [Header("Jump")]
    [SerializeField] float jumpVelocity = 6f;
    [SerializeField] Button jumpButton;

    CharacterController characterController;

    float vy = 0.0f;
    bool isJump = false;
    bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        var playerActionMap = inputActions.FindActionMap("Player");
        var playerAction = playerActionMap.FindAction("Move");

        jumpButton.onClick.AddListener(() =>
        {
            isJump = true;
        });

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask.value);

        if (isGrounded && vy < 0f)
        {
            vy = -2f;
        }

        if (isGrounded && isJump)
        {
            vy += jumpVelocity;
        }

        isJump = false;

        vy += gravity * Time.deltaTime;

        // move
        
        Vector3 horizontalDirection = transform.forward * variableJoystick.Vertical * speed * Time.deltaTime
                            + transform.right * variableJoystick.Horizontal * speed * Time.deltaTime;
        characterController.Move(horizontalDirection);

        characterController.Move(transform.up * vy * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask.value))
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.white;
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }

}
