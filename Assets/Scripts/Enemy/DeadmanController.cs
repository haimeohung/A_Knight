using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadmanController : EntityController
{
    private Transform playerPos;
    private PlayerControler2D player;
    private Animator ani;
    private Rigidbody2D rb;
    private bool OneTimeUpdate = true;
    private bool OneTimeUpdate2 = true;
    private State current_state;
    // serialize zone
    [SerializeField] float speed = 0f;
    [SerializeField] float range = 30;
    [SerializeField] bool _IsExit = true;
    [SerializeField] int random_attack;
    public EntityInfo info;
    // trigger zone
    private bool _IsAttack = false;
    private bool _IsFire = false;
    private bool _IsIdle = false;
    private bool _facingRight = true;

    private bool IsAttack
    {
        get => _IsAttack;
        set
        {
            if (value && !_IsAttack)
                ani?.SetTrigger("AttackTrigger");
            _IsAttack = value;
        }
    }

    private bool IsFire
    {
        get => _IsFire;
        set
        {
            if (value && !_IsFire)
                ani?.SetTrigger("FireTrigger");
            _IsFire = value;
        }
    }

    private bool IsIdle
    {
        get => _IsIdle;
        set
        {
            if (value && !_IsIdle)
                ani?.SetTrigger("IdleTrigger");
            _IsIdle = value;
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
    enum State
    {
        attack,
        fire,
        idle,
    }

    private void Start()
    {
        base.Start();

        ani = gameObject.GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerControler2D>();
        playerPos = player.transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        info = gameObject.GetComponent<EntityInfo>();
        rb.velocity = new Vector2(0f, 0f);
        StartCoroutine(FirstFrame());
    }
    private void SwitchState(State state)
    {
        current_state = state;
        if (state == State.attack)
        {
            IsAttack = true;
            IsFire = false;
            IsIdle = false;
            _IsExit = false;
            SoundManager.instance.Play("man_laugh");

        }

        if (state == State.fire)
        {
            IsAttack = false;
            IsFire = true;
            IsIdle = false;
            _IsExit = false;
        }

        if (state == State.idle)
        {
            IsAttack = false;
            IsFire = false;
            IsIdle = true;
            _IsExit = false;
        }
    }
    IEnumerator FirstFrame()
    {
        yield return 0;
        SwitchState(State.idle);
    }
    private void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0f, rb.velocity.y);
        if (_IsExit == false || info.HP_index <= 0)
        {
            return;
        }

        random_attack = (new System.Random()).Next(1, 4);

        if (random_attack == 1)
        {

            SwitchState(State.idle);
            SetVelocityIdle();

            return;
        }
        if (random_attack == 2)
        {

            SwitchState(State.attack);

            if (player.FacingRight)
            {
                rb.position = new Vector2(playerPos.position.x - 2, playerPos.position.y);
            }
            else
            {
                rb.position = new Vector2(playerPos.position.x + 2, playerPos.position.y);
            }
            return;
        }
        else
        {
            SwitchState(State.fire);
            return;
        }

    }
    public void SetVelocityIdle()
    {
        if (transform.position.x > playerPos.position.x)
        {
            FacingRight = false;

        }
        else
        {
            FacingRight = true;
        }
    }
    public void IsExit()
    {
        _IsExit = true;
    }

    private void LateUpdate()
    {

    }
}
