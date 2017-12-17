using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    float gravity = -1f;
    float speed = 5f;
    Vector2 velosity;
    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    void Update()
    {
        var inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velosity.x = inputDirection.x * speed * Time.deltaTime;
        velosity.y += gravity * Time.deltaTime;
//        print(velosity);
        controller.Move(velosity);
    }
}
