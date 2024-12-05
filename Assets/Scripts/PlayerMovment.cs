using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    [Header("Outside Inputs")]
    [SerializeField] ItemCheck itemCheck;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    Rigidbody2D rBody;
    PlayerControls playerControls;

    [Header("PlayerStats")]
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] bool doubleJump;
    float horizontal;

    [Header("Layer")]
    [SerializeField] int dashLayer;
    int originLayer;

    [Header("Dash")]
    [SerializeField] bool canDash;
    [SerializeField] bool isDashing;
    [SerializeField] float dashPower;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashBoost;
    float baseTime;

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

        playerControls.Player.Dash.performed += Dash;

        originLayer = gameObject.layer;

        baseTime = dashTime;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

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

        if(context.performed && !IsGrounded() && doubleJump && !isDashing)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, jumpPower);
            doubleJump = false;
        }

        if (context.canceled && rBody.velocity.y > 0)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y * 0.5f);
        }
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if(!itemCheck.GetDashCheck())
        {
            Debug.LogError("Cannot Dash");
            return;
        }

        if(context.performed && canDash)
        {
            StartCoroutine(DashMovement());
        }
    }

    #endregion

    private IEnumerator DashMovement()
    {
        canDash = false;
        isDashing = true;
        gameObject.layer = dashLayer;
        float originalGravity = rBody.gravityScale;
        rBody.gravityScale = 0f;
        rBody.velocity = new Vector2(transform.localScale.x * dashPower, 0f);

        //yield return new WaitForSeconds(dashTime);
        while (dashTime >= 0f)
        {
            dashTime -= Time.deltaTime;
            yield return null;
        }

        dashTime = baseTime;

        rBody.gravityScale = originalGravity;
        isDashing = false;
        gameObject.layer = originLayer;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterGate") && isDashing)
        {
            ExtendTime(0.1f);
        }
    }

    private void ExtendTime(float addTime)
    {
        dashTime += addTime;
    }
}
