using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

[RequireComponent(typeof(LineRenderer))]
public class WeaponBowControler : MonoBehaviour
{
    private UIInputHander input;
    private PlayerControler2D controler;
    [Header("Target")]
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;
    [SerializeField] Transform hand;
    private LineRenderer Renderer;
    void Start()
    {
        Renderer = GetComponent<LineRenderer>();
        Renderer.positionCount = 3;
        Renderer.SetWidth(0.075f, 0.075f);
        Renderer.SetColors(Color.black, Color.black);

        input = FindObjectOfType<Canvas>().GetComponentInChildren<UIInputHander>();
        controler = FindObjectOfType<PlayerControler2D>().GetComponent<PlayerControler2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Renderer.SetPosition(0, top.position);
        Renderer.SetPosition(1, hand.position);
        Renderer.SetPosition(2, bottom.position);

        Vector3 inputDirection = input.GetDirection(Unity.tag.JoystickTag.Weapon);
        if (inputDirection != Vector3.zero)
        {
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
