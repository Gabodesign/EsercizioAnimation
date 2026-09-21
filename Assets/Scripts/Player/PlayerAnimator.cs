using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator anim;
    public float acceleration = 5f;

    private static readonly int Move = Animator.StringToHash("Movimento"); 
    
    private Vector2 currentInput;
    private bool isRunning;
    private bool jump;
    private float currentSpeedValue;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove += UpdateMovementInput;
            InputManager.Instance.OnRun += UpdateRunInput;
            InputManager.Instance.OnJump += UpdateJump;
        }
    }

    void Update()
    {
        
        float targetSpeed = 0f;

        if (currentInput.magnitude > 0.1f)
        {
            //Controllo se il player sta camminando o correndo
            float speedLimit = isRunning ? 1f : 0.5f;

            targetSpeed = currentInput.magnitude * speedLimit;

            targetSpeed = Mathf.Clamp(targetSpeed, 0f, speedLimit);
        }

        currentSpeedValue = Mathf.MoveTowards(currentSpeedValue, targetSpeed, acceleration * Time.deltaTime);

        if (jump) 
        {
            anim.SetTrigger("Jump");
            jump = false;
        }


        // controllo se il player è fermo e cambio di posizione Idle
        if (currentInput == Vector2.zero && Mathf.Abs(currentSpeedValue) < 0.05f)
        {
            anim.SetFloat(Move, 0f);
        }
        else
        {
            anim.SetFloat(Move, currentSpeedValue);
            
        }
    }

    void UpdateMovementInput(Vector2 input)
    {
        currentInput = input;
    }

    void UpdateRunInput(bool run)
    {
        isRunning = run;
    }

    public void UpdateJump()
    {
        jump = true;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove -= UpdateMovementInput;
            InputManager.Instance.OnRun -= UpdateRunInput;
            InputManager.Instance.OnJump -= UpdateJump;
        }
    }
    
}