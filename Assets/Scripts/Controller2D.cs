using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class Controller2D : MonoBehaviour
{
    public float skinWidth;
    public LayerMask collisionMaskLayer;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public int numberRays = 5;

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

    void Update()
    {
//        RayArrayOrigin();
        print(skinWidth); 
    }

    void RayArrayOrigin()
    {
        var deltaX = (transform.localScale.x - skinWidth * 2) / (numberRays - 1);

        var deltaY = (transform.localScale.y - skinWidth * 2) / (numberRays - 1);
        var originRayY = transform.position.x - transform.localScale.x / 2 + skinWidth;
        var originRayX = new Vector2(transform.position.y - transform.localScale.y / 2 + skinWidth, transform.position.x);
        Vector2[] horizontalStartRays = new Vector2[numberRays];
        Vector2[] verticalStartRays = new Vector2[numberRays];
        for (int i = 0; i < numberRays; i++)
        {
            verticalStartRays[i] = new Vector2(originRayY + deltaY * i, transform.position.y);
//            horizontalStartRays[i] = new Vector2(originRayX + deltaX * i, transform.position.y);
            Debug.DrawRay(verticalStartRays[i], Vector2.up, Color.yellow, 5f);
        }

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

    void CollisionVertical(ref Vector2 velocity)
    {
        var direction = Mathf.Sign(velocity.y);
        var lengthRay = Mathf.Abs(velocity.y) + transform.localScale.y / 2;
        var hit = Physics2D.Raycast(transform.position, Vector2.up * direction, 100, collisionMaskLayer);
        if (hit)
        {
            velocity.y = hit.distance * direction;
        }

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
//                print(hit.distance); 
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
//        RayOrigin();
        UpdateRaycastOrigins();
        if (velosity.y != 0)
            VerticalCollisions(ref  velosity);
        if (velosity.x != 0)
            HorizontalCollisions(ref  velosity);
//        RayArrayOrigin();
        CollisionVertical(ref velosity);

        transform.Translate(velosity);

    }

}
