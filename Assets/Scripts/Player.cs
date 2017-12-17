using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    float gravity = -1f;
    Vector2 velosity;
    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    void Update()
    {
        velosity.y += gravity * Time.deltaTime;
        print(velosity);
        controller.Move(velosity * Time.deltaTime);
    }
}
