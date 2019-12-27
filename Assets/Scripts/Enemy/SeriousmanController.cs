using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeriousmanController : EntityController
{
    private Transform playerPos;
    private Animator ani;
    private Rigidbody2D rb;
    private bool OneTimeUpdate = true;
    private bool OneTimeUpdate2 = true;
    private State current_state;
    // serialize zone
    [SerializeField] float speed = 2f;
    [SerializeField] float range;
    [SerializeField] bool _IsExit = true;
    [SerializeField] int random_attack;
    public EntityInfo info;
    // trigger zone
    private bool _IsAttack = false;
    private bool _IsFury = false;
    private bool _IsFire = false;
    private bool _IsRunning = false;
    private bool _IsDie = false;
    private bool _IsIdle = false;
    private bool _IsLaugh = false;
    private bool _facingRight = true;
    
    private bool IsAttack
    {
        get => _IsAttack;
        set
        {
            if (value && !_IsAttack)
                ani.SetTrigger("AttackTrigger");
            _IsAttack = value;
        }
    }
    private bool IsFury
    {
        get => _IsFury;
        set
        {
            if (value && !_IsFury)
                ani.SetTrigger("FuryTrigger");
            _IsFury = value;
        }
    }
    private bool IsFire
    {
        get => _IsFire;
        set
        {
            if (value && !_IsFire)
                ani.SetTrigger("FireTrigger");
            _IsFire = value;
        }
    }
    private bool IsRunning
    {
        get => _IsRunning;
        set
        {
            if (value && !_IsRunning)
                ani.SetTrigger("RunningTrigger");
            _IsRunning = value;
        }
    }
    private bool IsDie
    {
        get => _IsDie;
        set
        {
            if (value && !_IsDie)
                ani.SetTrigger("DieTrigger");
            _IsDie = value;
        }
    }
    private bool IsIdle
    {
        get => _IsIdle;
        set
        {
            if (value && !_IsIdle)
                ani.SetTrigger("IdleTrigger");
            _IsIdle = value;
        }
    }
    private bool IsLaugh
    {
        get => _IsLaugh;
        set
        {
            if (value && !_IsLaugh)
                ani.SetTrigger("LaughTrigger");
            _IsLaugh = value;
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
        fury,
        fire,
        running,
        die,
        idle,
        laugh
    }
   
    private void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        playerPos = GameObject.FindObjectOfType<PlayerControler2D>().transform;
        info = gameObject.GetComponent<EntityInfo>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, 0f);
        StartCoroutine(FirstFrame());
    }
    private void SwitchState(State state)
    {
        current_state = state;
        if (state == State.attack)
        {
            IsAttack = true;
            IsFury = false;
            IsFire = false;
            IsRunning = false;
            IsDie = false;
            IsIdle = false;
            IsLaugh = false;

            _IsExit = false;
        }
        if (state == State.fury)
        {
            IsAttack = false;
            IsFury = true;
            IsFire = false;
            IsRunning = false;
            IsDie = false;
            IsIdle = false;
            IsLaugh = false;

            _IsExit = false;
        }
        if (state == State.fire)
        {
            IsAttack = false;
            IsFury = false;
            IsFire = true;
            IsRunning = false;
            IsDie = false;
            IsIdle = false;
            IsLaugh = false;

            _IsExit = false;
        }
        if (state == State.running)
        {
            IsAttack = false;
            IsFury = false;
            IsFire = false;
            IsRunning = true;
            IsDie = false;
            IsIdle = false;
            IsLaugh = false;

            _IsExit = false;
        }
        if (state == State.die)
        {
            IsAttack = false;
            IsFury = false;
            IsFire = false;
            IsRunning = false;
            IsDie = true;
            IsIdle = false;
            IsLaugh = false;

        }
        if (state == State.idle)
        {
            IsAttack = false;
            IsFury = false;
            IsFire = false;
            IsRunning = false;
            IsDie = false;
            IsIdle = true;
            IsLaugh = false;
            _IsExit = false;
        }
        if (state == State.laugh)
        {
            IsAttack = false;
            IsFury = false;
            IsFire = false;
            IsRunning = false;
            IsDie = false;
            IsIdle = false;
            IsLaugh = true;
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
        //base.Update();
        if (_IsExit == false )
        {
            if (OneTimeUpdate)
            {
                _IsExit = true;              
                OneTimeUpdate = false;
            }
            return;
        }
        if (Mathf.Abs(transform.position.x - playerPos.position.x) > range)
        {
            return;
        }
        if (OneTimeUpdate2 && info.HP_index <= 250)
        {
            SwitchState(State.fury);
            SwitchState(State.idle);
            SetVelocityIdle();
            OneTimeUpdate2 = false;
            return;
        }

        random_attack = (new System.Random()).Next(0, info.HP_index);
        if (random_attack % 10 == 0)
        {
            SwitchState(State.idle);
            SetVelocityIdle();
            return;
        }
        if (random_attack % 9 == 0)
        {
            SwitchState(State.running);
            if (transform.position.x > playerPos.position.x)
            {
                rb.velocity = new Vector2(-speed, 0f);

            }
            else
            {
                rb.velocity = new Vector2(speed, 0f);
            }
            return;
        }
        if (random_attack % 8 == 0 && info.HP_index > 250)
        {
            SwitchState(State.idle);
            SwitchState(State.laugh);
            SetVelocityIdle();
            return;
        }
        if (random_attack % 7 == 0)
        {
            SwitchState(State.attack);
            SetVelocityIdle();
            return;

        }
        else //(random_attack % 6 == 0)
        {
            SwitchState(State.fire);
            SetVelocityIdle();
            return;
        }
    
    }
    public void SetVelocityIdle()
    {
        if (transform.position.x > playerPos.position.x)
        {
            rb.velocity = new Vector2(-0.01f, 0f);
            FacingRight = false;

        }
        else
        {
            rb.velocity = new Vector2(0.01f, 0f);
            FacingRight = true;

        }
    }
    public void IsExit()
    {
        _IsExit = true;
    }

    private void LateUpdate()
    {
        if (current_state == State.running || _IsExit)
        {
            if (rb.velocity.x > 0)
            {
                FacingRight = true;
            }
            if (rb.velocity.x < 0)
            {
                FacingRight = false;
            }
        }
      
    }
}
