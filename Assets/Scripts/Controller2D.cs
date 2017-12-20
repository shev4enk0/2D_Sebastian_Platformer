using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Player))]
public class Controller2D : MonoBehaviour
{
    public float skinWidth = Mathf.Epsilon;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public LayerMask collisionMaskLayer;
    public CollisionInfo collisionInfo;

    float horizontalRaySpasing;
    float verticalRaySpasing;

    struct RaycastOrigins
    {
        public  Vector2 topLeft, topRight, bottomLeft, bottomRight;

    }

    public struct CollisionInfo
    {
        public bool above, below, left, right;

        public void Reset()
        {
            above = below = left = right = false;
        }

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
        bounds.Expand(-skinWidth * 2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpasing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(-skinWidth * 2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        verticalRaySpasing = (bounds.size.x) / (verticalRayCount - 1);
        horizontalRaySpasing = (bounds.size.y) / (horizontalRayCount - 1);
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        var direction = Mathf.Sign(velocity.y);
        var lengthRay = Mathf.Abs(velocity.y) + skinWidth;
       
        for (int i = 0; i < verticalRayCount; i++)
        {
            var rayOrigin = (direction == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

            rayOrigin += Vector2.right * (verticalRaySpasing * i);//+velosity.x

//            Debug.DrawRay(rayOrigin, Vector2.up * direction, Color.yellow);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction, lengthRay, collisionMaskLayer);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * direction;
                collisionInfo.below = direction == -1;
                collisionInfo.above = direction == 1;
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
//            Debug.DrawRay(rayOrigin, Vector2.right * direction * lengthRay, Color.yellow);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, lengthRay, collisionMaskLayer);
            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * direction;
                collisionInfo.left = direction == -1;
                collisionInfo.right = direction == 1;
            }
        }
    }

   

    public void Move(Vector2 velosity)
    {
        UpdateRaycastOrigins();
        collisionInfo.Reset();
        if (velosity.y != 0)
            VerticalCollisions(ref  velosity);
        if (velosity.x != 0)
            HorizontalCollisions(ref  velosity);

        transform.Translate(velosity);

    }

}
