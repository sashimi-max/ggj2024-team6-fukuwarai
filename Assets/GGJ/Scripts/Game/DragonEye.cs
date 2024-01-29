using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEye : MonoBehaviour
{
    private Vector2 eyeAcceleration;
    private Vector2 eyeVelocity;

    private void FixedUpdate()
    {
        eyeVelocity += eyeAcceleration - eyeVelocity * 0.05f;
        transform.localRotation = Quaternion.Euler(Vector3.forward * eyeVelocity.magnitude) * transform.localRotation;
        eyeAcceleration = Vector2.zero;
    }

    public void AddImpulseForce(Vector2 force)
    {
        eyeAcceleration = force;
    }
}
