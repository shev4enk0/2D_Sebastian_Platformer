using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class Controller2D : MonoBehaviour
{
    public float skinWidth = Mathf.Epsilon;
    public LayerMask collisionMaskLayer;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpasing;
    float verticalRaySpasing;

    struct RaycastOrigins
    {
        public  Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }


    BoxCollider2D boxCollider;
    RaycastOrigins raycastOrigins;

    void Start()
    {
        boxCollider = GetComponent < BoxCollider2D>();

        CalculateRaySpasing();
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x - skinWidth, bounds.min.y + skinWidth);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x - skinWidth, bounds.min.y + skinWidth);
        raycastOrigins.topLeft = new Vector2(bounds.min.x - skinWidth, bounds.max.y - skinWidth);
        raycastOrigins.topRight = new Vector2(bounds.max.x - skinWidth, bounds.max.y - skinWidth);
    }

    void CalculateRaySpasing()
    {
        Bounds bounds = boxCollider.bounds;

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        verticalRaySpasing = (bounds.size.x - skinWidth * 2) / (verticalRayCount - 1);
        horizontalRaySpasing = (bounds.size.y - skinWidth * 2) / (horizontalRayCount - 1);
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        var direction = Mathf.Sign(velocity.y);
        var lengthRay = Mathf.Abs(velocity.y) + skinWidth;
       
        for (int i = 0; i < verticalRayCount; i++)
        {
            var rayOrigin = (direction == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

            rayOrigin += Vector2.right * (verticalRaySpasing * i);//+velosity.x

            Debug.DrawRay(rayOrigin, Vector2.up * direction, Color.yellow);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction, lengthRay, collisionMaskLayer);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * direction;
                print(hit.distance); 
            }
        }
            
    }

    void HorizontalCollisions(ref Vector2 velocity)
    {
        var direction = Mathf.Sign(velocity.x);
        var lengthRay = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            var rayOrigin = (direction == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            rayOrigin += Vector2.up * (horizontalRaySpasing * i);
            Debug.DrawRay(rayOrigin, Vector2.right * direction * lengthRay, Color.yellow);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, lengthRay, collisionMaskLayer);
            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * direction;
            }

           
        }

    }

    public void Move(Vector2 velosity)
    {
        UpdateRaycastOrigins();
        if (velosity.y != 0)
            VerticalCollisions(ref  velosity);
        if (velosity.x != 0)
            HorizontalCollisions(ref  velosity);

        transform.Translate(velosity);

    }

}
