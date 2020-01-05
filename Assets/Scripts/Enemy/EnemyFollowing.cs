using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : MonoBehaviour
{
    private Transform playerPos;
    private Animator ani;
    private Rigidbody2D rb;
    [SerializeField] float speed = 1f;
    [SerializeField] float range = 30;
    [SerializeField] float rangeAttack = 5;
    public bool _IsExitAttack = true;
    private bool _IsFollowing = false;
    private bool IsFollowing
    {
        get => _IsFollowing;
        set
        {
            if (value && !_IsFollowing)
            {
                if (ani == null) return;
                ani?.SetTrigger("FollowingTrigger");
               
            }
            _IsFollowing = value;
        }
    }
    private bool _IsAttacking = false;
    private bool IsAttacking
    {
        get => _IsAttacking;
        set
        {
            if (value && !_IsAttacking)
            {
                if (ani == null) return;
                ani?.SetTrigger("AttackTrigger");
            }
            _IsAttacking = value;
        }
    }
    private bool IsDie;
    private bool _IsIdle = false;
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
    private int IsFinding;
    private float distance_finding;
    private float timer = -0f;
    private float timer2 = -0f;
    private float timeDelay = 2f;
    enum State
    {
        following,
        attacking,
        die,
        idle
    }
    private bool _facingRight = true;
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
    private void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        playerPos = GameObject.FindObjectOfType<PlayerControler2D>().transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, 0f);
        StartCoroutine(FirstFrame());
    }

    IEnumerator FirstFrame()
    {
        yield return 0;
        SwitchState(State.following);
    }

    private void SwitchState(State state)
    {
        if (ani is null) return;

        if (state == State.following)
        {
            IsFollowing = true;
            IsAttacking = false;
            IsDie = false;
            IsIdle = false;
            _IsExitAttack = false;

        }
        if (state == State.attacking)
        {
            IsFollowing = false;
            IsAttacking = true;
            IsDie = false;
            IsIdle = false;
            _IsExitAttack = false;

        }
        if (state == State.die)
        {
            IsFollowing = false;
            IsAttacking = false;
            IsDie = true;
            IsIdle = false;

        }
        if (state == State.idle)
        {
            IsFollowing = false;
            IsAttacking = false;
            IsDie = false;
            IsIdle = true;

        }

    }
    private void Update()
    {
        ///////
        float distance = Mathf.Abs(transform.position.x - playerPos.position.x);
        if (_IsExitAttack == false)
        {
            return;
        }
        if (distance >= range)
        {
            SwitchState(State.idle);
            rb.velocity = new Vector2(0f, 0f);
            return;
        }
        if (distance >= rangeAttack && distance < range - 2)
        {
            SwitchState(State.following);
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
        if (distance < rangeAttack - 2)
        {
            SwitchState(State.attacking);
            if (transform.position.x > playerPos.position.x)
            {
                rb.velocity = new Vector2(-0.01f, 0f);

            }
            else
            {
                rb.velocity = new Vector2(0.01f, 0f);
            }
            return;
        }


    }
    public void IsExitAttack()
    {
       
        _IsExitAttack = true;
    }
    private void LateUpdate()
    {
        if (IsFollowing || _IsExitAttack)
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