using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 3.0f;
    public float minJumpHeight = 1.0f;
    public float timeToApex = 0.33f;
    public float wallSlideMaxSpeed = 2.5f;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float accelerationTimeAirborne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;

    private float moveSpeed = 5.0f;
    private Vector3 velocity;
    private float velocityXSmoothed;

    private PlayerController controller;
    private Vector2 directionalInput;
    private SpriteRenderer playerSprite;

	void Start ()
    {
        controller = GetComponent<PlayerController>();
        playerSprite = GetComponent<SpriteRenderer>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

    void Update()
    {
        CalculateVelocity();

        if (transform.position.y >= 5.0f)
        {
            transform.position = new Vector3(transform.position.x, -4.25f, transform.position.z);
        }

        if (transform.position.y <= -5.0f)
        {
            transform.position = new Vector3(transform.position.x, 4.9f, transform.position.z);
        }

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            if(controller.collisionInfo.slidingDownMaxSlope)
            {
                velocity.y += controller.collisionInfo.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
            
        }

        // sprite flipping
        if(directionalInput.x > 0)
        {
            playerSprite.transform.localScale = new Vector3(2, 2, 1);
        }
        else if(directionalInput.x < 0)
        {
            playerSprite.transform.localScale = new Vector3(-2, 2, 1);
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        // regular jumping
        if (controller.collisionInfo.below)
        {
            if(controller.collisionInfo.slidingDownMaxSlope)
            {
                if(directionalInput.x != -Mathf.Sign(controller.collisionInfo.slopeNormal.x))
                {
                    velocity.y = maxJumpVelocity * controller.collisionInfo.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisionInfo.slopeNormal.x;
                }
            }
            else
            {
                velocity.y = maxJumpVelocity;
            }
            
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothed,
                                    (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}
