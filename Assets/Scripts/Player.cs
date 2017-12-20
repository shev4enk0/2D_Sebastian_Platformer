using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float jumpHeight = 4f;
    public float timeJumpApex = .4f;
    public float speedX = 5f;

    float smoothTimeInAir = 0.5f;
    float smoothTimeOnGround = 0.2f;
    float gravity;
    float jumpSpeed;
    float refVelocity;
   

    Vector2 velosity;
    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();

       
    }

    void Update()
    {
        var inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        gravity = -(jumpHeight * 2) / Mathf.Pow(timeJumpApex, 2);
        jumpSpeed = -gravity * timeJumpApex;
        print("gravity= " + gravity + "ApexTime= " + timeJumpApex);
       
        if (controller.collisionInfo.above || controller.collisionInfo.below)
            velosity.y = 0;
       
        var targetVelocity = inputDirection.x * speedX;
        velosity.x = Mathf.SmoothDamp(velosity.x, targetVelocity, ref refVelocity, controller.collisionInfo.below ? smoothTimeOnGround : smoothTimeInAir);
        velosity.y += gravity;
        if (Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below)
        {
            velosity.y = jumpSpeed;
        }
        controller.Move(velosity * Time.deltaTime);
    }
}
