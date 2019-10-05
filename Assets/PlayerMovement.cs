using UnityEngine;
using Luminosity.IO;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    public float timeToMaxAccel = 0.2f;
    public float timeToZeroAccel = 0.1f;
    public float timeToJumpHeight = 0.2f;
    public float timeToJumpFall = 0.2f;
    public float airControlFactor = 2;
    public AnimationCurve speedupCurve;
    public AnimationCurve slowdownCurve;
    public AnimationCurve jumpCurve;
    public AnimationCurve jumpFallCurve;

    Rigidbody body;
    Transform model;
    Vector3 direction;
    Vector3 speed;
    Camera cam;

    float x, y, z, verticalAxis, horizontalAxis, accelTime, slowdownTime;
    float acceleration, jumpAcceleration, jumpTime, jumpFallTime;
    bool isGrounded;
    bool isTouchingWall;
    bool isJumping;
    bool isSlamming;
    bool isDead;

    void Start()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isDead) {
            return;
        }

        isJumping = InputManager.GetButton("Jump") && isGrounded && jumpFallTime <= 0f && jumpTime <= 0f;
        verticalAxis = InputManager.GetAxisRaw("Vertical");
        horizontalAxis = InputManager.GetAxisRaw("Horizontal");

        x = (isGrounded) ? verticalAxis : verticalAxis / airControlFactor;
        z = (isGrounded) ? horizontalAxis : horizontalAxis / airControlFactor;

        RampAcceleration();
        print(jumpAcceleration);
        direction = cam.transform.TransformVector(new Vector3(z, 0, x));
        speed = direction.magnitude > 0 ? direction.normalized * moveSpeed * acceleration : Vector3.zero;

        body.velocity = new Vector3(speed.x, jumpAcceleration, speed.z) * Time.deltaTime;
    }

    void RampAcceleration()
    {
        if (Mathf.Abs(verticalAxis) > 0 || Mathf.Abs(horizontalAxis) > 0)
        {
            if (accelTime < timeToMaxAccel)
            {
                acceleration = speedupCurve.Evaluate(accelTime / timeToMaxAccel);
                accelTime += Time.fixedDeltaTime;
            }
            else
            {
                acceleration = 1f;
                slowdownTime = 0f;
            }
        }
        else
        {
            accelTime = 0f;
            if (slowdownTime < timeToZeroAccel)
            {
                acceleration = slowdownCurve.Evaluate(slowdownTime / timeToMaxAccel);
                slowdownTime += Time.fixedDeltaTime;
            }
            else
                acceleration = 0f;
        }

        if (isJumping || jumpTime > 0f) {
            if (jumpTime < timeToJumpHeight) {
                jumpAcceleration = jumpHeight * jumpCurve.Evaluate(jumpTime / timeToJumpHeight);
                jumpTime += Time.fixedDeltaTime;
            }
            else if (jumpTime >= timeToJumpHeight)
            {
                jumpTime = 0f;
                isJumping = false;
                jumpFallTime += Time.fixedDeltaTime;
            }
        }
        else if (jumpFallTime > 0f)
        {
            if (jumpFallTime < timeToJumpFall)
            {
                jumpAcceleration = jumpHeight * jumpFallCurve.Evaluate(jumpFallTime / timeToJumpFall);
                jumpFallTime += Time.fixedDeltaTime;
            }
        }
    }

    void OnCollisionStay(Collision col) {
        if (col.gameObject.CompareTag("Ground")) {
            isGrounded = true;
            jumpAcceleration = 0f;
            jumpFallTime = 0f;
        } else {
            isTouchingWall = true;
        }
    }

    void OnCollisionExit(Collision col) {
        if (col.gameObject.CompareTag("Ground")) {
            isGrounded = false;
        } else {
            isTouchingWall = false;
        }
    }
}
