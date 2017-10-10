using UnityEngine;
using System.Collections;

public class PlayerController : RaycastController
{
    public float maxSlopeAngle = 75.0f;

    public CollisionInfo collisionInfo;
    [HideInInspector]
    public Vector2 playerInput;



    public override void Start()
    {
        base.Start();
        collisionInfo.faceDirection = 1;

    }

    public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
       
        
    }

    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisionInfo.Reset();
        collisionInfo.moveAmountOld = moveAmount;
        playerInput = input;

        if(moveAmount.y < 0)
        {
           
        }

        if(moveAmount.x != 0)
        {
            collisionInfo.faceDirection = (int)Mathf.Sign(moveAmount.x);
        }




        HorizontalCollisions(ref moveAmount);
        
        if(moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }
  
        transform.Translate(moveAmount);

        if(standingOnPlatform)
        {
            collisionInfo.below = true;
        }
    }

    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);


            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if(hit)
            {
                if(hit.collider.tag == "CanPassThrough")
                {
                    if(directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if(collisionInfo.isFallingThrough)
                    {
                        continue;
                    }
                    if(playerInput.y == -1)
                    {
                        collisionInfo.isFallingThrough = true;
                        Invoke("ResetFallingThrough", 0.25f);
                        continue;
                    }
                    Invoke("ResetFallingThrough", 0.25f);
                }

                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if(collisionInfo.climbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisionInfo.below = directionY == -1;
                collisionInfo.above = directionY == 1;
            }
        }

        if(collisionInfo.climbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != collisionInfo.slopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisionInfo.slopeAngle = slopeAngle;
                    collisionInfo.slopeNormal = hit.normal;
                }
            }
        }
    }

    void HorizontalCollisions(ref Vector2 moveAmount)
    {
        //float directionX = Mathf.Sign(moveAmount.x);
        float directionX = collisionInfo.faceDirection;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if(Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                if(hit.distance == 0)
                {
                    continue;
                }

                if (collisionInfo.descendingSlope)
                {
                    collisionInfo.descendingSlope = false;
                    moveAmount = collisionInfo.moveAmountOld;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {

                }

                if (!collisionInfo.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisionInfo.climbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisionInfo.left = directionX == -1;
                    collisionInfo.right = directionX == 1;
                }

            }
        }
    }

    void ResetFallingThrough()
    {
        collisionInfo.isFallingThrough = false;
    }

    public struct CollisionInfo
    {
        public bool above, below, left, right;
        public bool climbingSlope, descendingSlope, slidingDownMaxSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector2 moveAmountOld;
        public int faceDirection;
        public bool isFallingThrough;
        public Vector2 slopeNormal;
        public bool isGrabbed;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
