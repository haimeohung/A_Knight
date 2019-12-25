using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class WeaponGunController : WeaponController
{
    private UIInputHander input;
    private PlayerControler2D controler;
    [SerializeField] private float bulletSpeed = 100f;
    private SpawnMachine spawn;
    private Vector3 inputDirection, lastInputDirection;
    void Start()
    {
        input = FindObjectOfType<UIInputHander>();
        controler = FindObjectOfType<PlayerControler2D>().GetComponent<PlayerControler2D>();
        spawn = gameObject.GetComponent<SpawnMachine>();
        spawn.SetOnInit += (clone) =>
        {
            clone.transform.eulerAngles = new Vector3(0, 0, lastInputDirection.signedAngle());
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.velocity = lastInputDirection.normalized * (-bulletSpeed);
        };
        spawn.SetOnDelete += (clone) =>
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
