using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

[RequireComponent(typeof(LineRenderer))]
public class WeaponBowControler : WeaponController
{
    private UIInputHander input;
    private PlayerControler2D controler;
    [SerializeField] private float arrowSpeed = 50f;
    [Header("Target")]
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;
    [SerializeField] Transform hand;
    private LineRenderer Renderer;
    private SpawnMachine spawn;
    private Vector3 inputDirection, lastInputDirection;
    void Start()
    {
        Renderer = GetComponent<LineRenderer>();
        Renderer.positionCount = 3;
        Renderer.SetWidth(0.075f, 0.075f);
        Renderer.SetColors(Color.black, Color.black);

        input = FindObjectOfType<UIInputHander>();
        controler = FindObjectOfType<PlayerControler2D>().GetComponent<PlayerControler2D>();
        spawn = gameObject.GetComponent<SpawnMachine>();
        spawn.SetOnInit = (clone) =>
        {
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.velocity = lastInputDirection.normalized * (-arrowSpeed);
        };
        spawn.SetOnDelete = (clone) =>
        {
            clone.slowFade();
        };

        SetOnTrigger = () =>
        {   
            spawn.Trigger_Spawn();
        };
    }
    
    void Update()
    {
        Renderer.SetPosition(0, top.position);
        Renderer.SetPosition(1, hand.position);
        Renderer.SetPosition(2, bottom.position);

        inputDirection = input.GetDirection(Unity.tag.JoystickTag.Weapon);
        if (inputDirection != Vector3.zero)
        {
            lastInputDirection = inputDirection;
            float angle = inputDirection.signedAngle();
            if (controler.FacingRight)
            {
                transform.localRotation = Quaternion.Euler(0, 0, angle + 180);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, -angle);
            }
        }
    }
}
