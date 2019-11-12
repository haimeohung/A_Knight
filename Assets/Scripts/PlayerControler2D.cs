using System.Collections;
using System.Collections.Generic;
using Unity.tag;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControler2D : MonoBehaviour
{
    [Header("Control Setting")]
    [SerializeField] private UIInputHander input = null;
    [SerializeField] private int runSpeed = 6;
    [SerializeField] private int jumpForce = 10;
    [SerializeField] private int jumpAbility = 1;
    [Range(0.2f, 0.5f)] [SerializeField] private float lengthTimeJump = 0.35f;
    [Header("Position Setting")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [Range(0.01f, 0.1f)] [SerializeField] private float checkRadius = 0.1f;
    [Range(0.0001f, 0.001f)] [SerializeField] private float accurate = 0.0005f;

    #region flag and get set
    [Header("Run-time Flag")]
    [SerializeField] private bool _facingRight = true;
    [SerializeField] private bool _isGrounder = false;
    [SerializeField] private bool _isJumpUp = false;
    [SerializeField] private bool _isJumpDown = false;
    [SerializeField] private int _runningSpeed = 0;
    [SerializeField] private int jumpCounter = 0;

    public bool FacingRight
    {
        get => _facingRight;
        private set
        {
            if (value != _facingRight)
            {
                transform.Rotate(Vector3.up * 180);
                _facingRight = value;
                foreach (var item in listCCD)
                    item.FlipAngleLimit();
            }
        }
    }
    public bool IsGrounder
    {
        get => _isGrounder;
        private set
        {
            if (value && (!_isGrounder))
                ani?.SetTrigger("Grounder");
            _isGrounder = value;
        }
    }
    public bool IsJumpUp
    {
        get => _isJumpUp;
        private set
        {
            if ((!_isJumpUp) && value)
                ani?.SetTrigger("JumpingUp");
            _isJumpUp = value;
        }
    }
    public bool IsJumpDown
    {
        get => _isJumpDown;
        private set
        {
            if ((!_isJumpDown) && value)
                ani?.SetTrigger("JumpingDown");
            _isJumpDown = value;
        }
    }
    public int RuningSpeed
    {
        get => _runningSpeed;
        private set
        {
            if (value == _runningSpeed)
                return;
            ani?.SetInteger("RunningSpeed", value);
            _runningSpeed = value;
            if (value != 0) 
            {
                if (value < 0)
                    FacingRight = false;
                else
                    FacingRight = true;
            }
            rb.velocity = new Vector2(value, rb.velocity.y);
        }
    }
    public bool IsJumpping => _isJumpUp || _isJumpDown;
    #endregion

    private Rigidbody2D rb;
    private Animator ani;
    private SimpleCCD[] listCCD;
    private float jumpTimer;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (input is null)
            input = FindObjectOfType<Canvas>().GetComponentInChildren<UIInputHander>();
        if (groundCheck is null)
            groundCheck = gameObject.GetComponentInChildren<Transform>();
        ani = GetComponent<Animator>();
        listCCD = GetComponentsInChildren<SimpleCCD>();
    }
    
    void Update()
    {
        IsGrounder = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (IsGrounder) 
        {
            jumpCounter = jumpAbility;
            if (input.OnButtonDown(ButtonTag.Jump))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimer = lengthTimeJump;
                jumpCounter--;
            }
            IsJumpUp = false;
            IsJumpDown = false;
        }
        else // On air
        {
            IsJumpUp = rb.velocity.y > accurate;
            IsJumpDown = rb.velocity.y < -accurate;

            if (input.IsPress(ButtonTag.Jump))
                if (jumpTimer > 0f)
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                else // additional jump
                {
                    if (jumpCounter > 0 && input.OnButtonDown(ButtonTag.Jump))
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                        jumpTimer = lengthTimeJump;
                        jumpCounter--;
                    }
                }
            jumpTimer -= Time.fixedDeltaTime;
        }

        RuningSpeed = input.movingDirection * runSpeed;
    }   

    void FixedUpdate()
    {
        if (IsJumpDown)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y > -jumpForce ? rb.velocity.y : -jumpForce);
    }
}
