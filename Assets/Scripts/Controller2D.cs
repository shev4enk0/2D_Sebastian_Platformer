using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Controller2D : MonoBehaviour
{
    struct RaycastOrigins
    {
        public  Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    BoxCollider2D boxCollider;

    // Use this for initialization
    void Start()
    {
        boxCollider = GetComponent < BoxCollider2D>();
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }

}
