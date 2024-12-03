using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    [Header("Outside Inputs")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    Rigidbody2D rBody;
    PlayerControls playerControls;

    [Header("PlayerStats")]
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] bool doubleJump;
    float horizontal;

    #region Cutscene
    public bool SetCutscene(bool inCutscene)
    {
        if(inCutscene)
        {
            horizontal = 0;
            playerControls.Player.Disable();
        }
        else
        {
            playerControls.Player.Enable();
        }

        return inCutscene;
    }
    #endregion

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();

        playerControls.Player.Enable();

        playerControls.Player.Movement.performed += Move;
        playerControls.Player.Movement.canceled += Move;

        playerControls.Player.Jump.performed += Jump;
        playerControls.Player.Jump.canceled += Jump;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void FixedUpdate()
    {
        rBody.velocity = new Vector2(horizontal * playerSpeed, rBody.velocity.y);

        if(IsGrounded())
        {
            doubleJump = true;
        }
    }

    private void Flip() { transform.localScale = new Vector3(1 * horizontal, 1, 1); }

    private bool IsGrounded() { return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer); }

    #region InputControls
    private void Move(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            horizontal = context.ReadValue<Vector2>().x;
            Flip();
        }

        if (context.canceled)
        {
            horizontal = 0;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && IsGrounded())
        {
            rBody.velocity = new Vector2(rBody.velocity.x, jumpPower);
        }

        if(context.performed && !IsGrounded() && doubleJump)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, jumpPower);
            doubleJump = false;
        }

        if (context.canceled && rBody.velocity.y > 0)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y * 0.5f);
        }
    }
    #endregion
}
