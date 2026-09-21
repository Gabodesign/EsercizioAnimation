using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settaglio Movimento, salto e rotazione")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;
    [Header("Settings camera")]
    [SerializeField] private float sensitivity = 30f;

    private Rigidbody rb;
    private Vector2 direction;
    private Vector2 lookInput;
    private bool isRunning;
    private bool isGrounded;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove += HandleMoveInput;
            InputManager.Instance.OnRun += HandleRunToggle;
            InputManager.Instance.OnJump += HandleJumpInput;
            InputManager.Instance.OnLook += HandleRotateCamInput;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove -= HandleMoveInput;
            InputManager.Instance.OnRun -= HandleRunToggle;
            InputManager.Instance.OnJump -= HandleJumpInput;
            InputManager.Instance.OnLook -= HandleRotateCamInput;
        }
    }

    void Start()
    {
        if (InputManager.Instance != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void FixedUpdate()
    {
        ApplyRotation();
    }


    private void HandleMoveInput(Vector2 input) => direction = input;
    private void HandleRotateCamInput(Vector2 input) => lookInput = input;
    public void HandleRunToggle(bool runState) => isRunning = runState;
    
    public void HandleJumpInput() // controllo se sta toccando o meno il terreno
    {
        if (isGrounded)
        {
            jumpRequested = true;
        }
    }
    public bool HasMovementInput() => direction != Vector2.zero;
    public bool HasRotateCamInput() => lookInput != Vector2.zero;
    public bool IsRunningInput() => isRunning;

    public void ApplyMove() //applico il movimento 
    {
        if (direction == Vector2.zero)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        float targetSpeed = isRunning ? runSpeed : walkSpeed;


        Vector3 moveDirection = (transform.forward * direction.y) + (transform.right * direction.x);

        // Normalizziamo per evitare che cammini più velocemente in diagonale
        moveDirection.Normalize();
        Vector3 targetVelocity = moveDirection * targetSpeed;

        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
    }


    public void ApplyRotation() // applico la rotazione della telecamera
    { 
        if (lookInput.magnitude < 0.01f) return;

        float mouseX = lookInput.x * sensitivity * Time.fixedDeltaTime;

        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}