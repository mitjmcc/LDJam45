using UnityEngine;
using Luminosity.IO;

public class CameraFollow : MonoBehaviour
{
    public Transform FollowObject;
    public Transform showOffLocation;
    public Rigidbody PlayerBody;
    public Vector3 offset;

    bool showOff = false;

    void FixedUpdate()
    {
        if (!showOff)
            transform.position = Vector3.Lerp(transform.position, FollowObject.position + PlayerBody.velocity * 0.1f + offset, 20f * Time.fixedDeltaTime);
        if (Input.GetKeyDown("v"))
        {   
            if (!showOff)
                ShowOffLantern();
            else
                Reset();
        }
    }

    [System.Obsolete]
    void ShowOffLantern()
    {
        showOff = true;
        FollowObject.GetComponent<PlayerMovement>().enabled = false;
        FollowObject.forward = Vector3.right;
        FollowObject.position = showOffLocation.position;
        transform.position = showOffLocation.position + new Vector3(0, 2f, -2f);
        transform.rotation.SetEulerAngles(30, 0 ,0);
    }

    void Reset()
    {
        showOff = false;
        FollowObject.GetComponent<PlayerMovement>().enabled = true;
        FollowObject.position = Vector3.zero;
        transform.position = FollowObject.position + offset;
    }
}
