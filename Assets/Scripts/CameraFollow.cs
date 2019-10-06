using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FollowObject;
    public Rigidbody PlayerBody;
    public Vector3 offset;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, FollowObject.position + PlayerBody.velocity * 0.1f + offset, 20f * Time.fixedDeltaTime);
    }
}
