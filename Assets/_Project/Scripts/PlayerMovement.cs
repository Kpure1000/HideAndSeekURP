using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input System Config")]
    [SerializeField] InputActionAsset inputActions;


    [Header("Move And Run")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] VariableJoystick moveControlJoystick;
    [SerializeField] Button runButton;


    [Header("Gravity")]
    [SerializeField] float gravity = -22f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;


    [Header("Jump")]
    [SerializeField] float jumpVelocity = 8f;
    [SerializeField] Button jumpButton;


    [Header("Player States")]
    [SerializeField]
    private float m_Speed = 0f;
    [SerializeField]
    private bool m_isJump = false;
    [SerializeField]
    private bool m_isRunning = false;
    [SerializeField]
    private bool m_isGrounded = false;
    [SerializeField]
    private bool m_isMoving = false;


    public float Speed { get => m_Speed; set => m_Speed = value; }
    public bool IsJump { get => m_isJump; set => m_isJump = value; }
    public bool IsRunning { get => m_isRunning; set => m_isRunning = value; }
    public bool IsGrounded { get => m_isGrounded; set => m_isGrounded = value; }
    public bool IsMoving { get => m_isMoving; set => m_isMoving = value; }


    private CharacterController characterController;

    private float vy = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        var playerActionMap = inputActions.FindActionMap("Player");
        var playerAction = playerActionMap.FindAction("Move");

        AddJumpButtonEvent();
        AddRunButtonEvent();

    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask.value);

        if (IsGrounded && vy < 0f)
        {
            vy = -2f;
        }

        if (IsGrounded && IsJump)
        {
            vy += jumpVelocity;
        }

        IsJump = false;

        vy += gravity * Time.deltaTime;

        // move

        m_Speed = IsRunning ? runSpeed : moveSpeed;

        Vector3 horizontalDirection = transform.forward * moveControlJoystick.Vertical * m_Speed * Time.deltaTime
                            + transform.right * moveControlJoystick.Horizontal * m_Speed * Time.deltaTime;
        characterController.Move(horizontalDirection);

        characterController.Move(transform.up * vy * Time.deltaTime);

        m_isMoving = moveControlJoystick.Vertical > 0.1f || moveControlJoystick.Horizontal > 0.1f;
    }

    void AddJumpButtonEvent()
    {
        EventTrigger jumpButtonTrigger = jumpButton.gameObject.AddComponent<EventTrigger>();

        {
            EventTrigger.Entry jumpTrigger = new EventTrigger.Entry();
            jumpTrigger.eventID = EventTriggerType.PointerDown;
            jumpTrigger.callback.AddListener((BaseEventData eventData) =>
            {
                IsJump = true;
            });
            jumpButtonTrigger.triggers.Add(jumpTrigger);
        }

        {
            EventTrigger.Entry jumpEndTrigger = new EventTrigger.Entry();
            jumpEndTrigger.eventID = EventTriggerType.PointerUp;
            jumpEndTrigger.callback.AddListener((BaseEventData eventData) =>
            {
                IsJump = false;
            });
            jumpButtonTrigger.triggers.Add(jumpEndTrigger);
        }
    }

    void AddRunButtonEvent()
    {
        EventTrigger runButtonTrigger = runButton.gameObject.AddComponent<EventTrigger>();
        {
            EventTrigger.Entry runStartTrigger = new EventTrigger.Entry();
            runStartTrigger.eventID = EventTriggerType.PointerDown;
            runStartTrigger.callback.AddListener((BaseEventData eventData) =>
            {
                IsRunning = true;
            });
            runButtonTrigger.triggers.Add(runStartTrigger);
        }
        {
            EventTrigger.Entry runEndTrigger = new EventTrigger.Entry();
            runEndTrigger.eventID = EventTriggerType.PointerUp;
            runEndTrigger.callback.AddListener((BaseEventData eventData) =>
            {
                IsRunning = false;
            });
            runButtonTrigger.triggers.Add(runEndTrigger);
        }
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
