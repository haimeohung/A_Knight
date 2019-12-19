using System.Collections;
using System.Collections.Generic;
using Unity.tag;
using Unity.Extentison;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerControler2D : MonoBehaviour
{
    [Header("Control Setting")]
    [SerializeField] private UIInputHander input = null;
    [SerializeField] private int runSpeed = 6;
    [SerializeField] private int jumpForce = 10;
    [SerializeField] private int jumpAbility = 1;
    [SerializeField] private UISliderController HP, MP, SP;
    [Range(0.2f, 0.5f)] [SerializeField] private float lengthTimeJump = 0.35f;
    [Header("Checking Setting")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck = null;
    [SerializeField] private float checkRadius = 0.1f;
    [Range(0.0001f, 0.001f)] [SerializeField] private float accurate = 0.0005f;
    [Header("Weapon Setting")]
    [SerializeField] private Transform weaponContainer = null;
    [SerializeField] private Transform leftHandTarget = null;
    [SerializeField] private Transform rightHandTarget = null;
    #region flag and get set
    [Header("Info run-time Flag (Read-Only, do not edit here)")]
    [SerializeField] private WeaponTag _selectedWeapon = WeaponTag.None;
    [SerializeField] private int _attackPhase = 0;
    [SerializeField] private bool _facingRight = true;
    [SerializeField] private bool _isGrounder = false;
    [SerializeField] private bool _isJumpUp = false;
    [SerializeField] private bool _isJumpDown = false;
    [SerializeField] private bool _canAttack = false;
    [SerializeField] private bool _attacking = false;
    [SerializeField] private int _runningSpeed = 0;
    [SerializeField] private int jumpCounter = 0;
    public Vector2 Carring { get; set; }
    public int AttackPhase
    {
        get => _attackPhase;
        private set
        {
            if (_attackPhase == value)
                return;
            CanGoNextAttack = false;
            _attackPhase = value > 3 ? 1 : value;
            ani.SetInteger("AttackPhase", _attackPhase);
        }
    }
    public bool FacingRight
    {
        get => _facingRight;
        private set
        {
            if (value != _facingRight)
            {
                transform.Rotate(Vector3.up * 180);
                _facingRight = value;
            }
        }
    }
    public bool IsGrounder
    {
        get => _isGrounder;
        private set
        {
            if (value && (!_isGrounder))
            {
                ani?.SetTrigger("Grounder");
                Attacking = false;
            }
            _isGrounder = value;
        }
    }
    public bool IsJumpUp
    {
        get => _isJumpUp;
        private set
        {
            if ((!_isJumpUp) && value)
                if (SelectedWeapon == WeaponTag.Sword)
                {
                    if (!Attacking)
                        ani?.SetTrigger("JumpingUp");
                }
                else
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
                if (SelectedWeapon == WeaponTag.Sword)
                {
                    if (!Attacking)
                        ani?.SetTrigger("JumpingDown");
                }
                else
                    ani?.SetTrigger("JumpingDown");
            _isJumpDown = value;
        }
    }
    public bool CanAttack
    {
        get => _canAttack;
        set
        {
            if (!_canAttack && value)
            {
                if (!Attacking && IsGrounder)
                    AttackPhase++;
                Attacking = true;
            }
            _canAttack = value;
        }
    }
    public bool Attacking
    {
        get => _attacking;
        set
        {
            if (!_attacking && value)
                ani.SetTrigger("Attack");
            _attacking = value;
        }
    }
    public int RuningSpeed
    {
        get => _runningSpeed;
        private set
        {
            rb.velocity = new Vector2(value, rb.velocity.y);
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
    public WeaponTag SelectedWeapon
    {
        get => _selectedWeapon;
        set
        {
            if (_selectedWeapon == value)
                return;
            weapons[(int)_selectedWeapon]?.parent.SetActive(false);
            try
            {
                weapons[(int)value].parent.SetActive(true);
                _selectedWeapon = value;
                ani.SetInteger("SelectedWeapon", (int)value);
            }
            catch
            {
                Debug.LogError("Weapon not exist!");
            }
            switch (value)
            {
                case WeaponTag.Bow:
                case WeaponTag.Gun:
                    input.AttackMode = AttackMode.HaveDirection;
                    break;
                default:
                    input.AttackMode = AttackMode.NonDirection;
                    break;
            }
        }
    }
    #endregion


    private Rigidbody2D rb;
    private Animator ani;
    private float jumpTimer;
    private Nodes[] weapons;

    [System.Serializable] class Nodes
    {
        public Transform left = null, right = null;
        public GameObject parent = null;
        public Nodes() { }
        public Nodes(GameObject weapon)
        {
            parent = weapon;
            parent.SetActive(false);
            left = parent.transform.GetChild(0);
            right = parent.transform.GetChild(1);
        }
        public Nodes(Transform weapon)
        {
            parent = weapon.gameObject;
            parent.SetActive(false);
            left = parent.transform.GetChild(0);
            right = parent.transform.GetChild(1);
        }
    }

    void Start()
    {
        #region init
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (input is null)
            input = FindObjectOfType<UIInputHander>();
        if (HP is null || MP is null || SP is null)
        {
            UISliderController[] scrolls = FindObjectsOfType<UISliderController>();
            foreach (UISliderController item in scrolls)
                switch (item.gameObject.name)
                {
                    case "HP":
                        HP = item;
                        break;
                    case "MP":
                        MP = item;
                        break;
                    case "SP":
                        SP = item;
                        break;
                }
            if (HP is null || MP is null || SP is null)
                Debug.LogError("Cannt find HP stat");
        }
        if (groundCheck.isNull())
        {
            groundCheck = transform.Find("GroundCheck")?.GetComponent<Transform>();
            if (groundCheck.isNull())
            {
                GameObject gc = new GameObject("GroundCheck");
                gc.transform.SetParent(transform);
                groundCheck = gc.transform;
                CircleCollider2D cr = gameObject.GetComponent<CircleCollider2D>();
                if (cr != null)
                {
                    groundCheck.localPosition = cr.bounds.center + new Vector3(cr.offset.x, cr.offset.y);
                    checkRadius = cr.radius * gameObject.transform.localScale.x * 1.1f;
                }
            }
        }

        BoxCollider2D boxCollider2D = transform.GetComponent<BoxCollider2D>();
        groundCheck.localPosition = new Vector3(0, boxCollider2D.offset.y - boxCollider2D.size.y * 0.5f, 0);
        ani = GetComponent<Animator>();

        weapons = new Nodes[System.Enum.GetValues(typeof(WeaponTag)).Length];
        if (weaponContainer.isNull())
        {
            weaponContainer = transform.Find("WeaponContainer");
            if (weaponContainer.isNull())
            {
                Debug.LogError("Not assign WeaponContainer!");
                throw new System.Exception("Not assign WeaponContainer!");
            }
        }
        foreach (Transform child in weaponContainer)
        {
            Nodes node = new Nodes(child);
            var tag = node.parent.GetComponent<WeaponType>()?.tag;
            if (!(tag is null))
                weapons[(int)tag] = node;
        }
        #endregion
        SelectedWeapon = WeaponTag.Bow;
    }

    void Update()
    {
        RuningSpeed = input.movingDirection * runSpeed;
        IsGrounder = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        #region Jump
        if (IsGrounder)
        {
            if (!input.IsPress(ButtonTag.Jump))
                jumpCounter = jumpAbility;
            if (input.OnButtonDown(ButtonTag.Jump) && jumpCounter > 0 && SP.value > 5) 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimer = lengthTimeJump;
                jumpCounter--;
                SP.value -= 5;
            }
            IsJumpUp = false;
            IsJumpDown = false;
        }
        else // On air
        {
            IsJumpUp = rb.velocity.y > accurate;
            IsJumpDown = rb.velocity.y < -accurate;
            if (IsJumpDown && jumpCounter == jumpAbility)
                jumpCounter--;

            if (input.IsPress(ButtonTag.Jump))
            {
                if (jumpTimer > 0f)
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                else // additional jump
                {
                    if (jumpCounter > 0 && input.OnButtonDown(ButtonTag.Jump) && SP.value > 20)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                        jumpTimer = lengthTimeJump;
                        jumpCounter--;
                        SP.value -= 20;
                    }
                }
            }
            else
                jumpTimer = -0f;
            jumpTimer -= Time.deltaTime;
        }
        #endregion
        CanAttack = input.OnButtonDown(ButtonTag.Attack);
        Attacking = input.GetDirection(JoystickTag.Weapon).Magnitude2D() > 0.1f;
        CanGoNextAttack = input.OnButtonDown(ButtonTag.Attack);
    }
    private void LateUpdate()
    {
        RuningSpeed = input.movingDirection * runSpeed;
        if (IsJumpDown)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y > -jumpForce * 2 ? rb.velocity.y : -jumpForce * 2);
        rb.velocity += Carring;
        if (!(IsJumpDown || IsJumpUp))
            _isGrounder = true;
        

        #region weapon controler
        if (input.AttackMode == AttackMode.HaveDirection)
            try
            {
                leftHandTarget.position = weapons[(int)SelectedWeapon].left.position;
                rightHandTarget.position = weapons[(int)SelectedWeapon].right.position;
            }
            catch
            {
                if (Time.frameCount % 60 == 0)
                    Debug.LogWarning("Null object");
            }
        #endregion
    }

    void AttackDone()
    {
        Attacking = false;
        
        if (IsGrounder && CanGoNextAttack)
        {
            AttackPhase++;
            Attacking = true;
        }
        else
        {
            if (input.GetDirection(JoystickTag.Weapon).Magnitude2D() < 0.1)
                ani.SetTrigger("AttackDone");
            AttackPhase = 0;
        }
    }

    void AttackSpawnType()
    {
        weapons[(int)SelectedWeapon]?.parent.GetComponent<WeaponController>()?.SetOnTrigger();
    }

    #region combo attack
    private bool _canGoNextAttack = false;
    private float privaterTimer = -0f;
    private float attackDelay = 0.2f;
    public bool CanGoNextAttack
    {
        get => _canGoNextAttack;
        set
        {
            if (value)
            {
                _canGoNextAttack = value;
                if (privaterTimer <= 0)
                    StartCoroutine(DenyAttack());
                else
                    privaterTimer = attackDelay;
            }
            
        }
    }
    IEnumerator DenyAttack()
    {
        privaterTimer = attackDelay;
        while (privaterTimer > 0)
        {
            privaterTimer -= Time.deltaTime;
            yield return 0;
        }
        _canGoNextAttack = false;
    }
#endregion
}